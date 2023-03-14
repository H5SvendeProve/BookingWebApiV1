using BookingWebApiV1.Api.Mappers;
using BookingWebApiV1.Api.Requests;
using BookingWebApiV1.Authentication;
using BookingWebApiV1.Database;
using BookingWebApiV1.Encryption;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Services.LoginService;

public class LoginService : ILoginService
{
    private IDatabaseContext DatabaseContext { get; }
    private IJwtProvider JwtProvider { get; }
    
    private IRequestMapper RequestMapper { get; }
    public LoginService(IDatabaseContext databaseContext, IJwtProvider jwtProvider, IRequestMapper requestMapper)
    {
        DatabaseContext = databaseContext;
        JwtProvider = jwtProvider;
        RequestMapper = requestMapper;
    }

    public async Task<bool> LoginUser(LoginUserRequest userRequest)
    {
        var potentialUser = await DatabaseContext.GetUserWithGivenUsername(userRequest.Username);

        bool isPasswordMatch = false;
        

        if (potentialUser.Username == userRequest.Username)
        {
            isPasswordMatch = Hasher.ValidatePassword(userRequest.Password, potentialUser);
        }

        return isPasswordMatch;

    }

    public Task<bool> Logout(string username)
    {
        throw new System.NotImplementedException();
    }

    public async Task<bool> RegisterNewUser(CreateNewUserRequest userRequest)
    {
        var userDTO = RequestMapper.MapRequestToDTO(userRequest);
        
        var userExists = await DatabaseContext.UserExists(userDTO.Username);

        bool userRegistered = false;

        if (!userExists)
        {

            var userSalt = Hasher.GenerateSalt();

            var hashedPassword = Hasher.SaltAndHashPassword(userDTO.Password, userSalt);

            userDTO.PasswordSalt = userSalt;

            userDTO.Password = hashedPassword;

            userRegistered = await DatabaseContext.InsertNewUser(userDTO);

        }

        return userRegistered;
    }

    public Task<bool> Delete(UserDTO user)
    {
        throw new System.NotImplementedException();
    }

    public async Task<string> GenerateJwtToken(string username)
    {
        var token = await JwtProvider.GenerateNewToken(username);

        return token;
    }
}