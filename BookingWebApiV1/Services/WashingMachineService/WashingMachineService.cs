using BookingWebApiV1.Api.Mappers;
using BookingWebApiV1.Database;
using BookingWebApiV1.Exceptions;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;
using Microsoft.IdentityModel.Tokens;
using ArgumentException = BookingWebApiV1.Exceptions.ArgumentException;

namespace BookingWebApiV1.Services.WashingMachineService;

public class WashingMachineService : IWashingMachineService
{
    private IDatabaseContext DatabaseContext { get; }
    
    private IRequestMapper RequestMapper { get; }
    
    
    public WashingMachineService(IDatabaseContext databaseContext, IRequestMapper requestMapper)
    {
        DatabaseContext = databaseContext;
        RequestMapper = requestMapper;
    }

    public async Task<BookingDTO> GetBookingConnectedToRfid(string rfidCardId, DateTime scannedTime)
    {

        if (string.IsNullOrEmpty(rfidCardId)|| string.IsNullOrWhiteSpace(rfidCardId))
        {
            throw new ArgumentException("rfid cannot be null or empty");
        }

        var rfidCard = await DatabaseContext.GetRfidCard(rfidCardId);

        if (rfidCard.Equals(default(RfidCardDTO)))
        {
            throw new NotFoundException("rfid not presented in database");
        }

        var bookingsFromDb = await DatabaseContext.GetBookedBookingBasedOnRfidCard(rfidCard, scannedTime);

        if (bookingsFromDb.IsNullOrEmpty())
        {
            throw new NotFoundException("no bookings created to be started at this time");
        }

        return bookingsFromDb.First();
    }
    

    public async Task<ProgramResultDTO> GetBookingProgram(BookingDTO bookingDTO)
    {
        if (bookingDTO.BookingId < 1)
        {
            // handle
        }

        var program = await DatabaseContext.GetProgram(bookingDTO);

        return program;
    }

    public async Task<bool> RfidCardExists(string rfidCardId)
    {
        if (string.IsNullOrEmpty(rfidCardId) || string.IsNullOrWhiteSpace(rfidCardId))
        {
            throw new ArgumentException($"Argument cannot be null or empty {nameof(rfidCardId)}");
        }
        
        var rfidCardExists = await DatabaseContext.RfidCardExists(rfidCardId);

        return rfidCardExists;
    }
}