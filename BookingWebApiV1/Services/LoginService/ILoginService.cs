using BookingWebApiV1.Api.Requests;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Services.LoginService;

public interface ILoginService
{
    public Task<bool> LoginUser(LoginUserRequest userRequest);
    public Task<bool> Logout(string username);
    public Task<bool> RegisterNewUser(CreateNewUserRequest userRequest);
    public Task<bool> Delete(UserDTO user);
    public Task<string> GenerateJwtToken(string username);
}