using BookingWebApiV1.Api.Requests;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Services.AngularService;

public interface IAngularService
{
    Task<List<ElectricityPriceDTO>> GetPrices();

    Task<bool> CreateNewMachine(CreateNewMachineRequest createNewMachineRequest);

    Task<BookingDTO> CreateNewBooking(CreateNewBookingRequest createNewBookingRequest);

    Task<bool> ValidateToken(string token);
    
    Task<bool> CreateNewRfidCard(CreateNewRfidCardRequest rfidCardRequest);

    Task<MasterArduinoDTO> CreateNewMasterArduino(CreateMasterArduinoRequest createMasterArduinoRequest);
    Task<ArduinoMachineDTO> CreateNewArduinoMachine(CreateArduinoMachineRequest createArduinoMachineRequest);
    Task<List<ArduinoMachineDTO>> GetMachinesByArduinoMasterId(string arduinoMasterId);
}