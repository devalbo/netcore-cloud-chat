using ChatBackend.Db.Models;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ChatBackend.Db.Repositories
{
    public interface IRoomLatestActivityRepository
    {
        Task UpdateRoomActivity(int dbRoomId, int messageId);
        Task<List<DbRoomLatestActivity>> GetAllDbRoomLatestActivity();
    }

    public class RoomLatestActivityRepository: IRoomLatestActivityRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly IRoomRepository _roomRepository;

        public RoomLatestActivityRepository(
            IDbConnectionFactory dbConnectionFactory, 
            IRoomRepository roomRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _roomRepository = roomRepository;
        }

        public async Task UpdateRoomActivity(int dbRoomId, int messageId)
        {
            var now = DateTimeOffset.UtcNow;
            var roomExists = await _roomRepository.DoesRoomExistById(dbRoomId);
            if (!roomExists)
            {
                throw new ApplicationException($"No room to update activity for [{dbRoomId}]");
            }

            using var db = _dbConnectionFactory.OpenDbConnection();
            var roomActivity = await db.SelectAsync<DbRoomLatestActivity>(a => a.DbRoomId == dbRoomId);

            if (!roomActivity.Any())
            {
                await db.InsertAsync(new DbRoomLatestActivity()
                {
                    LatestActivityDateTime = now,
                    DbRoomId = dbRoomId,
                    MessageId = 0
                });
            }
            else
            {
                var latestActivity = await db.SingleAsync<DbRoomLatestActivity>(r => r.DbRoomId == dbRoomId);
                latestActivity.LatestActivityDateTime = now;
                latestActivity.MessageId = messageId;
                await db.SaveAsync(latestActivity);
            }
        }

        public async Task<List<DbRoomLatestActivity>> GetAllDbRoomLatestActivity()
        {
            using var db = _dbConnectionFactory.OpenDbConnection();
            var allActivity = await db.SelectAsync<DbRoomLatestActivity>();
            return allActivity.ToList();
        }
    }
}
