using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Services.LoginService;

public interface ILoginService
{
    public Task<string> LoginUser(LoginUserRequest userRequest);
    public Task<bool> Logout(string username);
    public Task<bool> RegisterNewUser(CreateNewUserRequest userRequest);
    public Task<bool> Delete(UserDTO user);
}