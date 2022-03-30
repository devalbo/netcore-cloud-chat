using ChatBackend.Auth;
using ChatBackend.Controllers.Helpers;
using ChatBackend.Db.Repositories;
using ChatBackend.Dto;
using ChatBackend.Middleware.Auth;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ChatBackend.Controllers
{
    [ApiController]
    [Route("rooms")]
    [EnableCors]
    public class RoomsController: ControllerBase
    {
        private readonly ILoggedInUserProvider _loggedInUserProvider;
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomLatestActivityRepository _roomLatestActivityRepository;
        private readonly IRoomsByActivityProvider _roomsByActivityProvider;

        public RoomsController(
            ILoggedInUserProvider loggedInUserProvider, 
            IRoomRepository roomRepository,
            IRoomsByActivityProvider roomsByActivityProvider, 
            IRoomLatestActivityRepository roomLatestActivityRepository)
        {
            _loggedInUserProvider = loggedInUserProvider;
            _roomRepository = roomRepository;
            _roomsByActivityProvider = roomsByActivityProvider;
            _roomLatestActivityRepository = roomLatestActivityRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<Room>> GetAllRoomsByActivity()
        {
            var allRooms = await _roomsByActivityProvider.GetAllDbRoomsByActivity();
            return allRooms;
        }

        [HttpPost]
        [Authorize]
        [Route("new")]
        public async Task<CreateNewRoomResponse> CreateNewRoom([FromBody] CreateNewRoomRequest request)
        {
            var requestedRoomName = request.Name.Trim();

            var roomNameExists = await _roomRepository.DoesRoomNameExist(requestedRoomName);
            if (roomNameExists)
            {
                return new CreateNewRoomResponse()
                {
                    Success = false,
                    FailureReason = $"Room name {requestedRoomName} already exists"
                };
            }

            var loggedInUser = _loggedInUserProvider.GetUserForCurrentRequest(this);
            var createdId = await _roomRepository.CreateRoom(requestedRoomName, loggedInUser.UserDbId);

            await _roomLatestActivityRepository.UpdateRoomActivity(createdId, 0);

            return new CreateNewRoomResponse()
            {
                Success = true,
                Id = createdId,
            };
        }
    }
}
