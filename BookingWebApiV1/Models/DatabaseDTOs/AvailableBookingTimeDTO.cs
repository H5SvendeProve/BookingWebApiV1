namespace BookingWebApiV1.Models.DatabaseDTOs;

public class AvailableBookingTimeDTO
{
    public int bookingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string DepartmentName { get; set; }
    
}