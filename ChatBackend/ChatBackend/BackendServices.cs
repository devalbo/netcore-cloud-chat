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
            //var dbFactory = CreateSqliteConnectionFactory();
            var dbFactory = CreatePostgresConnectionFactory();
            
            services.AddSingleton<IDbConnectionFactory>(dbFactory);

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

        private static OrmLiteConnectionFactory CreatePostgresConnectionFactory()
        {
            // I know, I know... this should go in a config somewhere, at some point
            var connectionString = "User ID=postgres;Password=postgrespasswd;Host=localhost;Port=15432;Database=chat_app;Pooling=true;Connection Lifetime=0;";

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
