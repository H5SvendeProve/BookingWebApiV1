using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.Requests;

public class CreateNewMachineRequest
{
    [Required]
    public string MachineManufacturer { get; init; }
    [Required]
    public string ModelName { get; init; }
    [Required]
    public double Effect { get; init; }
    
}