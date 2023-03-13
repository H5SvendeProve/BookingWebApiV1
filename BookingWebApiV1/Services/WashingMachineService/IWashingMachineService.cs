using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Services.WashingMachineService;

public interface IWashingMachineService
{
    Task<BookingDTO> GetBookingConnectedToRfid(string rfidCardId);
    Task<bool> RfidCardExists(string rfidCardId);
}