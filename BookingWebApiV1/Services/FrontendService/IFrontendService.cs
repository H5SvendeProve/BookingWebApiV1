using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Services.FrontendService;

public interface IFrontendService
{
    Task<bool> CreateNewMachine(CreateNewMachineRequest createNewMachineRequest);

    Task<BookingDTO> CreateNewBooking(CreateNewBookingRequest createNewBookingRequest);

    Task<bool> ValidateToken(string token);
    
    Task<bool> CreateNewRfidCard(CreateNewRfidCardRequest rfidCardRequest);

    Task<MasterArduinoDTO> CreateNewMasterArduino(CreateMasterArduinoRequest createMasterArduinoRequest);
    Task<ArduinoMachineDTO> CreateNewArduinoMachine(CreateArduinoMachineRequest createArduinoMachineRequest);
    Task<List<ArduinoMachineDTO>> GetMachinesByArduinoMasterId(string arduinoMasterId);
    Task<List<AvailableBookingTimeDTO>> GetAvailableBookingTimes(string username);
    Task<List<BookingDTO>> GetUserBookings(string username);
    Task<List<ProgramDTO>> GetMachineProgramsFromMachine(string machineManufacturer, string machineModelName, string machineType);
}