using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.Requests;

public class CreateMasterArduinoRequest
{
    [Required]
    public string DepartmentName { get; set; }
}