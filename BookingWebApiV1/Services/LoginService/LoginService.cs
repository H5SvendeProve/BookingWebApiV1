using BookingWebApiV1.Api.Mapper;
using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Authentication;
using BookingWebApiV1.Database;
using BookingWebApiV1.Encryption;
using BookingWebApiV1.Exceptions;
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

    public async Task<string> LoginUser(LoginUserRequest userRequest)
    {
        var potentialUser = await DatabaseContext.GetUserWithGivenUsername(userRequest.Username);

        string jwtToken = string.Empty;
        
        if (string.IsNullOrEmpty(userRequest.Username) || string.IsNullOrWhiteSpace(userRequest.Username))
        {
            throw new BadRequestException("Username is required");
        }

        if (string.IsNullOrEmpty(userRequest.Password) || string.IsNullOrWhiteSpace(userRequest.Password))
        {
            throw new BadRequestException("Password is required");
        }

        if (userRequest.Username != potentialUser.Username)
        {
            throw new NotFoundException($"Username or password is incorrect");
        }
        
        bool isPasswordMatch = false;


        if (potentialUser.Username == userRequest.Username)
        {
            isPasswordMatch = Hasher.ValidatePassword(userRequest.Password, potentialUser);
        }

        if (!isPasswordMatch)
        {
            throw new BadRequestException("username or password is incorrect");
        }

        jwtToken = await JwtProvider.GenerateNewToken(userRequest.Username);

        if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new BadRequestException("error happened during generation of jwt token");
        }

        return jwtToken;
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
}