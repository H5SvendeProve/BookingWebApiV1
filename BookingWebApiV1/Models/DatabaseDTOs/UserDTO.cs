namespace BookingWebApiV1.Models.DatabaseDTOs;

public class UserDTO
{
    public string Username { get; init; }
    public string Password { get; set; }
    public string PasswordSalt { get; set; }
    public UserRole UserRole { get; set; }

    public string DepartmentName { get; set; }
}

public enum UserRole
{
    Standard,
    Admin
}