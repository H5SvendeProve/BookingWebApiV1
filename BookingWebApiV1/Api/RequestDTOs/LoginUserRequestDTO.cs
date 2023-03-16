using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.RequestDTOs;

public class LoginUserRequest
{
    [Required]
    public string Username { get; }
    [Required]
    public string Password { get; }

    public LoginUserRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}