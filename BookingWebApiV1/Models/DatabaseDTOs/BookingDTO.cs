namespace BookingWebApiV1.Models.DatabaseDTOs;

public class BookingDTO
{
    public int BookingId { get; set; }
    public string Username { get; set; }
    public decimal Price { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ProgramId { get; set; }
    public string MachineManufacturer { get; set; }
    public string ModelName { get; set; }
}