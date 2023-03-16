namespace BookingWebApiV1.Models.DatabaseResultDTOs;

public class ProgramResultDTO
{
    public string ProgramName { get; set; }
    public int ProgramRunTimeMinutes { get; set; }
    public string MachineManufacturer { get; set; }
    public string ModelName { get; set; }
    
    public string MachineType { get; set; }
}