using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.RequestDTOs;

public class CreateMasterArduinoRequest
{
    [Required]
    public string DepartmentName { get; set; }
}