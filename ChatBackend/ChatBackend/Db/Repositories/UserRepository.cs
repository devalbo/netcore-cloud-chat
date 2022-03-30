using ChatBackend.Db.Models;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ChatBackend.Db.Repositories
{
    public interface IUserRepository
    {
        Task<int> AddUserToDb(string userName, string screenName);
        Task<DbUser?> GetDbUserForUserName(string userName);
        Task<Dictionary<int, DbUser>> GetUsersForIds(IEnumerable<int> messageUserIds);
    }

    public class UserRepository: IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> AddUserToDb(string userName, string screenName)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();
            
            var usersForUserName = await db.SelectAsync<DbUser>(u => u.UserName == userName);
            if (usersForUserName.Count > 0)
            {
                throw new ApplicationException($"DbUser with name {userName} already exists");
            }

            var userId = await db.InsertAsync(new DbUser()
            {
                UserName = userName,
                ScreenName = screenName,
                CreationDateTime = DateTimeOffset.UtcNow,
            }, true);

            return (int)userId;
        }
        
        public async Task<DbUser?> GetDbUserForUserName(string userName)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var userForUserName = await db.SingleAsync<DbUser>(u => u.UserName == userName);

            return userForUserName;
        }
        
        public async Task<Dictionary<int, DbUser>> GetUsersForIds(IEnumerable<int> userIds)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var users = await db.SelectByIdsAsync<DbUser>(userIds);
            if (users == null)
            {
                return new Dictionary<int, DbUser>();
            }

            var usersByIds = users.ToDictionary(m => m.Id, m => m);

            return usersByIds;
        }
    }
}
