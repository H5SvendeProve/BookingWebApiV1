using BookingWebApiV1.Api.Requests;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;

namespace BookingWebApiV1.Database;

public interface IDatabaseContext
{
    Task<List<ElectricityPriceDTO>> GetElectricityPrices();
    Task<UserDTO> GetUserWithGivenUsername(string username);
    Task<bool> UserExists(string username);

    Task<bool> InsertNewUser(UserDTO user);

    Task<bool> InsertNewMachine(MachineDTO machineDTO);

    Task<BookingDTO> InsertNewBooking(BookingDTO bookingDTO);

    Task<BookingMachineProgramDTO> GetBookingMachineProgramFromBooking(BookingDTO bookingDTO);

    Task<List<BookingElectricityPriceDTO>> GetElectricityPricesBasedOnBooking(BookingDTO bookingDTO);

    Task<bool> InsertNewRfidCard(RfidCardDTO rfidCardDTO);

    Task<bool> RfidCardExists(string rfidCardId);

    Task<RfidCardDTO> GetRfidCard(string rfidCardId);

    Task<MasterArduinoDTO> GetMasterArduino(string? masterArduinoId, string? apiKey);

    Task<List<BookingDTO>> GetBookedBookingBasedOnRfidCard(RfidCardDTO rfidCardDTO);

    Task<MasterArduinoDTO> InsertNewMasterArduino(MasterArduinoDTO masterArduinoDTO);

    Task<ArduinoMachineDTO> InsertNewArduinoMachine(ArduinoMachineDTO arduinoMachineDTO);
    Task<List<ArduinoMachineDTO>> GetMachinesByArduinoMasterId(string arduinoMasterId);
}