namespace BookingWebApiV1.Models.DatabaseDTOs;

public class MachineDTO
{
    public string MachineManufacturer { get; set; }
    public string ModelName { get; set; }
    public double EffectKWh { get; set; }

    public string MachineType { get; set; }
}