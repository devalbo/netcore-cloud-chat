using ChatBackend.Db.Models;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ChatBackend.Db.Repositories
{
    public interface IMessageRepository
    {
        Task<int> CreateMessage(string content, int roomId, int userDbId);
        List<DbMessage> GetMessagesForRoom(int roomId, int count);
        DbMessage? GetLatestMessageForRoom(int roomId);
        Task<Dictionary<int, DbMessage>> GetMessagesForIds(List<int> latestRoomMessageIds);
    }

    public class MessageRepository : IMessageRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MessageRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }
        
        public async Task<int> CreateMessage(string content, int roomId, int userDbId)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var now = DateTimeOffset.UtcNow;
            var messageId = await db.InsertAsync(new DbMessage()
            {
                PostedByUserId = userDbId,
                RoomId = roomId,
                Content = content,
                CreationDateTime = now,
            }, true);

            return (int)messageId;
        }

        public List<DbMessage> GetMessagesForRoom(int roomId, int count)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var result = db
                .SelectLazy(db.From<DbMessage>()
                    .Where(m => m.RoomId == roomId)
                    .OrderBy(m => m.CreationDateTime)
                    .Limit(count)
                )
                .ToList();

            return result;
        }

        public DbMessage? GetLatestMessageForRoom(int roomId)
        {
            var messages = GetMessagesForRoom(roomId, 1);
            return messages.SingleOrDefault();
        }

        public async Task<Dictionary<int, DbMessage>> GetMessagesForIds(List<int> messageIds)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var messages = await db.SelectByIdsAsync<DbMessage>(messageIds);
            if (messages == null)
            {
                return new Dictionary<int, DbMessage>();
            }

            var messagesByIds = messages.ToDictionary(m => m.Id, m => m);

            return messagesByIds;
        }
    }
}
