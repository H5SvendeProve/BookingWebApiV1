using System.ComponentModel.DataAnnotations;

namespace BookingWebApiV1.Api.Requests;

public class CreateArduinoMachineRequest
{
    [Required]
    public string MasterArduinoId { get; set; }
    [Required]
    public string MachineManufacturer { get; set; }
    [Required]
    public string ModelName { get; set; }
}