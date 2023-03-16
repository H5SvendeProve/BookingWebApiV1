using System.ComponentModel.DataAnnotations;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Api.RequestDTOs;

public class CreateNewUserRequest
{
    [Required] public string Username { get; init; }
    [Required] public string Password { get; init; }
    [Required] public UserRole UserRole { get; init; }
    [Required] public string DepartmentName  { get; init; }
}