using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;

namespace BookingWebApiV1.Services.WashingMachineService;

public interface IWashingMachineService
{
    Task<BookingDTO> GetBookingConnectedToRfid(string rfidCardId);

    Task<ProgramResultDTO> GetBookingProgram(BookingDTO bookingDTO);
    
    Task<bool> RfidCardExists(string rfidCardId);
}