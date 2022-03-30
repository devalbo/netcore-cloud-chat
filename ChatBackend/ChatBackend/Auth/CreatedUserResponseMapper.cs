using ChatBackend.Dto;

namespace ChatBackend.Auth
{
    public interface ICreatedUserResponseMapper
    {
        CreatedUserResponse Map(CreateUserResponse createUserResponse);
    }

    public class CreatedUserResponseMapper: ICreatedUserResponseMapper
    {
        public CreatedUserResponse Map(CreateUserResponse createUserResponse)
        {
            return new CreatedUserResponse()
            {
                Success = true,
                Id = createUserResponse.Id,
                FinalUserName = createUserResponse.FinalUserName
            };
        }
    }
}
