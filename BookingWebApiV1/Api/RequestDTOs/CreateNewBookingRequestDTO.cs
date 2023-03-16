using System.ComponentModel.DataAnnotations;
using BookingWebApiV1.Api.Validation;

namespace BookingWebApiV1.Api.RequestDTOs;

public class CreateNewBookingRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    [TimeValidation(ErrorMessage = "Start time must be in the future")]
    public DateTime StartTime { get; set; }
    [Required]
    public int ProgramId { get; init; }
    [Required]
    public string MachineManufacturer { get; init; }
    [Required]
    public string ModelName { get; init; }
}