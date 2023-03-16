using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.RequestDTOs;

public class CreateNewRfidCardRequest
{
    [Required]
    public string RfidCardId { get; init; }
    [Required]
    public string Username { get; init; }
}