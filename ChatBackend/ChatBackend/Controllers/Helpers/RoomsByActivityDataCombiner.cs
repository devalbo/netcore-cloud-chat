using ChatBackend.Db.Models;
using ChatBackend.Dto;

namespace ChatBackend.Controllers.Helpers
{
    public interface IRoomsByActivityDataCombiner
    {
        List<Room> Combine(RoomsByActivityData roomsByActivityData);
    }

    public class RoomsByActivityDataCombiner: IRoomsByActivityDataCombiner
    {
        public List<Room> Combine(RoomsByActivityData roomsByActivityData)
        {
            var orderedAllActivity = roomsByActivityData
                .AllActivityByRooms
                .Values
                .OrderByDescending(a => a.LatestActivityDateTime);
            var activityOrderedRooms = orderedAllActivity
                .Select(a => roomsByActivityData.AllDbRooms[a.DbRoomId]);

            return activityOrderedRooms
                .Select(r => Map(r, roomsByActivityData))
                .ToList();
        }

        private Message? GetMostRecentMessageForRoom(DbRoom room, RoomsByActivityData roomsByActivityData)
        {
            var roomId = room.Id;
            var latestRoomMessageId = roomsByActivityData.AllActivityByRooms[roomId].MessageId;

            if (latestRoomMessageId == 0)
            {
                return null;
            }

            var latestMessage = roomsByActivityData.LatestRoomMessages[latestRoomMessageId]!;
            var latestMessageContent = latestMessage.Content;
            var latestMessageSender = MapUser(latestMessage.PostedByUserId, roomsByActivityData);
            
            return new Message()
            {
                Id = latestMessage.Id,
                Content = latestMessageContent,
                SentAt = latestMessage.CreationDateTime,
                SentBy = latestMessageSender,
            };
        }

        private User MapUser(int userId, RoomsByActivityData roomsByActivityData)
        {
            var userName = roomsByActivityData.UsersForMessages[userId].UserName;

            return new User()
            {
                Id = userId,
                Name = userName
            };
        }

        private Room Map(DbRoom room, RoomsByActivityData roomsByActivityData)
        {
            var mostRecentMessage = GetMostRecentMessageForRoom(room, roomsByActivityData);

            return new Room()
            {
                Id = room.Id,
                Name = room.Name,
                MostRecentMessage = mostRecentMessage
            };
        }
    }
}
