using ChatBackend.Db.Models;
using ChatBackend.Db.Repositories;
using ChatBackend.Dto;

namespace ChatBackend.Controllers.Helpers
{
    public class RoomsByActivityData
    {
        public Dictionary<int, DbRoomLatestActivity> AllActivityByRooms { get; set; }
        public Dictionary<int, DbMessage> LatestRoomMessages { get; set; }
        public Dictionary<int, DbUser> UsersForMessages { get; set; }
        public Dictionary<int, DbRoom> AllDbRooms { get; set; }
    }

    public interface IRoomsByActivityProvider
    {
        Task<List<Room>> GetAllDbRoomsByActivity();
    }

    public class RoomsByActivityProvider: IRoomsByActivityProvider
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomLatestActivityRepository _roomLatestActivityRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomsByActivityDataCombiner _roomsByActivityDataCombiner;

        public RoomsByActivityProvider(
            IRoomRepository roomRepository, 
            IRoomLatestActivityRepository roomLatestActivityRepository, 
            IRoomsByActivityDataCombiner roomsByActivityDataCombiner, 
            IMessageRepository messageRepository, 
            IUserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _roomLatestActivityRepository = roomLatestActivityRepository;
            _roomsByActivityDataCombiner = roomsByActivityDataCombiner;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Room>> GetAllDbRoomsByActivity()
        {
            var allActivityTask = _roomLatestActivityRepository.GetAllDbRoomLatestActivity();
            var allDbRoomsTask = _roomRepository.GetAllRooms();

            await Task.WhenAll(allActivityTask, allDbRoomsTask);

            var allActivityByRooms = allActivityTask
                .Result
                .ToDictionary(a => a.DbRoomId, a => a);
            var allDbRooms = allDbRoomsTask.Result.ToDictionary(r => r.Id, r => r);

            var latestRoomMessageIds = allActivityByRooms
                .Values
                .Select(a => a.MessageId)
                .Where(mid => mid != 0)
                .ToList();

            var messages = await _messageRepository.GetMessagesForIds(latestRoomMessageIds);

            var messageUserIds = messages.Values.Select(m => m.PostedByUserId);
            var usersForMessages = await _userRepository.GetUsersForIds(messageUserIds);

            var roomsByActivityData = new RoomsByActivityData()
            {
                AllActivityByRooms = allActivityByRooms,
                AllDbRooms = allDbRooms,
                LatestRoomMessages = messages,
                UsersForMessages = usersForMessages,
            };

            var roomsByActivity = _roomsByActivityDataCombiner.Combine(roomsByActivityData);

            return roomsByActivity;
        }
    }
}
