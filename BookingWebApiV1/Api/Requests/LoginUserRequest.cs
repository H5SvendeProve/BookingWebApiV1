using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.Requests;

public class LoginUserRequest
{
    [Required]
    public string Username { get; init; }
    [Required]
    public string Password { get; init; }
}