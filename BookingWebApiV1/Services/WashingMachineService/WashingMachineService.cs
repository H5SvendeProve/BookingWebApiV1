﻿using BookingWebApiV1.Api.Mappers;
using BookingWebApiV1.Database;
using BookingWebApiV1.Models.DatabaseDTOs;

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

    public async Task<BookingDTO> GetBookingConnectedToRfid(string rfidCardId)
    {
        var rfidCard = await DatabaseContext.GetRfidCard(rfidCardId);

        if (string.IsNullOrEmpty(rfidCardId))
        {
            throw new ArgumentException("rfid missing in database");
        }

        var bookingsFromDb = await DatabaseContext.GetBookedBookingBasedOnRfidCard(rfidCard);

        return bookingsFromDb.First();
    }

    public async Task<bool> RfidCardExists(string rfidCardId)
    {
        var rfidCardExists = await DatabaseContext.RfidCardExists(rfidCardId);

        return rfidCardExists;
    }
}