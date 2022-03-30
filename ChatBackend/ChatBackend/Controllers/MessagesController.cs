using ChatBackend.Auth;
using ChatBackend.Db.Models;
using ChatBackend.Db.Repositories;
using ChatBackend.Dto;
using ChatBackend.Middleware.Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatBackend.Controllers
{
    [ApiController]
    [Route("messages")]
    [EnableCors]
    public class MessagesController: ControllerBase
    {
        private readonly ILoggedInUserProvider _loggedInUserProvider;
        private readonly IRoomRepository _roomRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomLatestActivityRepository _roomLatestActivityRepository;


        public MessagesController(
            ILoggedInUserProvider loggedInUserProvider,
            IMessageRepository messageRepository, 
            IRoomLatestActivityRepository roomLatestActivityRepository, 
            IRoomRepository roomRepository, 
            IUserRepository userRepository)
        {
            _loggedInUserProvider = loggedInUserProvider;
            _messageRepository = messageRepository;
            _roomLatestActivityRepository = roomLatestActivityRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Authorize]
        [Route("new")]
        public async Task<CreateNewMessageResponse> CreateNewMessage([FromBody] CreateNewMessageRequest request)
        {
            var roomExists = await _roomRepository.DoesRoomExistById(request.RoomId);
            if (!roomExists)
            {
                throw new ApplicationException($"Invalid room ID: {request.RoomId}");
            }

            var loggedInUser = _loggedInUserProvider.GetUserForCurrentRequest(this);
            var messageId = await _messageRepository.CreateMessage(request.Contents, request.RoomId, loggedInUser.UserDbId);

            await _roomLatestActivityRepository.UpdateRoomActivity(request.RoomId, messageId);

            return new CreateNewMessageResponse()
            {
                Success = true,
                MessageId = messageId
            };
        }

        [HttpGet]
        [Authorize]
        [Route("room/{roomId}")]
        public async Task<List<Message>> GetMessagesForRoom([FromRoute] int roomId)
        {
            var roomExists = await _roomRepository.DoesRoomExistById(roomId);
            if (!roomExists)
            {
                return new List<Message>();
            }

            var dbMessages = _messageRepository.GetMessagesForRoom(roomId, 50);
            var dbMessageSenderUserIds = dbMessages.Select(m => m.PostedByUserId).Distinct();
            var userIdsToDbUsers = await _userRepository.GetUsersForIds(dbMessageSenderUserIds);
            
            var messages = dbMessages
                .Select(m => new Message()
                {
                    Content = m.Content,
                    Id = m.Id,
                    SentBy = MapUser(userIdsToDbUsers[m.PostedByUserId]),
                    SentAt = m.CreationDateTime
                }).ToList();

            return messages;
        }

        private User MapUser(DbUser user)
        {
            return new User()
            {
                Id = user.Id,
                Name = user.ScreenName
            };
        }
    }
}
