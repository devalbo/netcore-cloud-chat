using ChatBackend.Db.Models;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ChatBackend.Db.Repositories
{
    public interface IRoomRepository
    {
        Task<bool> DoesRoomNameExist(string roomName);
        Task<bool> DoesRoomExistById(int roomId);
        Task<int> CreateRoom(string roomName, int userDbId);
        Task<List<DbRoom>> GetAllRooms();
    }

    public class RoomRepository: IRoomRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public RoomRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> DoesRoomNameExist(string roomName)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var roomsWithName = await db.SelectAsync<DbRoom>(u => u.Name == roomName);
            return roomsWithName.Any();
        }

        public async Task<bool> DoesRoomExistById(int roomId)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var rooms = await db.SelectAsync<DbRoom>(x => x.Id == roomId);

            return rooms.Any();
        }

        public async Task<int> CreateRoom(string roomName, int userDbId)
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var now = DateTimeOffset.UtcNow;
            var roomId = await db.InsertAsync(new DbRoom()
            {
                CreatedByUserId = userDbId,
                Name = roomName,
                CreationDateTime = now,
            }, true);

            return (int)roomId;
        }

        public async Task<List<DbRoom>> GetAllRooms()
        {
            using var db = _dbConnectionFactory.OpenDbConnection();

            var allRooms = await db.SelectAsync<DbRoom>();
            return allRooms;
        }
    }
}
