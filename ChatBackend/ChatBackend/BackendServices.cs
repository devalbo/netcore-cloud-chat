using ChatBackend.Auth;
using ChatBackend.Controllers.Helpers;
using ChatBackend.Db.Models;
using ChatBackend.Db.Repositories;
using ChatBackend.Middleware.Auth;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ChatBackend
{
    public class BackendServices
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IUserAccessTokenFactory, UserAccessTokenFactory>();
            services.AddScoped<IUserNameSanitizer, UserNameSanitizer>();
            services.AddScoped<ILoginOrchestrator, LoginOrchestrator>();
            services.AddScoped<ILogoutOrchestrator, LogoutOrchestrator>();
            services.AddScoped<ILoggedInUserProvider, LoggedInUserProvider>();
            services.AddScoped<IUserCreator, UserCreator>();
            services.AddScoped<ICreatedUserResponseMapper, CreatedUserResponseMapper>();
            services.AddScoped<IRoomsByActivityProvider, RoomsByActivityProvider>();
            services.AddScoped<IRoomsByActivityDataCombiner, RoomsByActivityDataCombiner>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IRoomLatestActivityRepository, RoomLatestActivityRepository>();

            services.AddSingleton<IUserAccessAuthorizationProvider, UserAccessAuthorizationProvider>();
        }

        public static void ConfigureDb(IServiceCollection services, bool resetDb = false)
        {
            bool isHostedInDockerCompose = true;

            //var dbFactory = CreateSqliteConnectionFactory();
            var dbFactory = CreatePostgresConnectionFactory(isHostedInDockerCompose);
            
            var dbInitStartTime = DateTimeOffset.UtcNow;
            var dbInitGiveUpTime = dbInitStartTime.AddMinutes(1);

            while (DateTimeOffset.UtcNow < dbInitGiveUpTime)
            {
                try
                {
                    InitDb(dbFactory, resetDb);
                    services.AddSingleton<IDbConnectionFactory>(dbFactory);
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR INITIALIZING DB...");
                    Console.WriteLine($"Using connection string >> {dbFactory.ConnectionString}");
                    Console.WriteLine("We'll try again in a little while");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);

                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }

            // docker compose + postgres issues? We'll at least try a temporary in-memory DB with Sqlite
            Console.WriteLine("Using Sqlite DB as DB of last resort");
            var sqliteDbFactory = CreateSqliteConnectionFactory();
            InitDb(sqliteDbFactory, false);
            services.AddSingleton<IDbConnectionFactory>(sqliteDbFactory);
        }

        public static void InitDb(OrmLiteConnectionFactory dbFactory, bool resetDb)
        {
            using var db = dbFactory.Open();
            if (resetDb)
            {
                db.DropTables();
            }

            db.CreateTableIfNotExists<DbUser>();
            db.CreateTableIfNotExists<DbRoom>();
            db.CreateTableIfNotExists<DbMessage>();
            db.CreateTableIfNotExists<DbRoomLatestActivity>();
        }

        // I know, I know... this should go in a config somewhere, at some point
        private static string GetPostgresConnectionString(bool hostedInDockerCompose)
        {
            var postGresUser = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var postGresUserPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "postgrespasswd";
            var postGresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "postgres";
            var port = hostedInDockerCompose ? 5432 : 15432;

            var connectionString = $"User ID={postGresUser};Password={postGresUserPassword};Host={postGresHost};Port={port};Database=chat_app;Pooling=true;Connection Lifetime=0;";
            return connectionString;
        }

        private static OrmLiteConnectionFactory CreatePostgresConnectionFactory(bool hostedInDockerCompose)
        {
            var connectionString = GetPostgresConnectionString(hostedInDockerCompose);
            var dbFactory = new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider);
            return dbFactory;
        }

        private static OrmLiteConnectionFactory CreateSqliteConnectionFactory()
        {
            var dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);
            return dbFactory;
        }  
    }
}
