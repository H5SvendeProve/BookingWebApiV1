using System.ComponentModel.DataAnnotations;
using BookingWebApiV1.Api.Validation;

namespace BookingWebApiV1.Api.Requests;

public class CreateNewBookingRequest
{
    [Required]
    public string Username { get; init; }
    [Required]
    [TimeValidation(ErrorMessage = "Start time must be in the future")]
    public DateTime StartTime { get; init; }
    [Required]
    public int ProgramId { get; init; }
    [Required]
    public string MachineManufacturer { get; init; }
    [Required]
    public string ModelName { get; init; }
}