using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;

namespace BookingWebApiV1.Database;

public interface IDatabaseContext
{
    Task<List<ElectricityPriceDTO>> GetElectricityPrices(string username);
    Task<UserDTO> GetUserWithGivenUsername(string username);
    Task<bool> UserExists(string username);

    Task<bool> InsertNewUser(UserDTO user);

    Task<bool> InsertNewMachine(MachineDTO machineDTO);

    Task<BookingDTO> InsertNewBooking(BookingDTO bookingDTO);

    Task<List<AvailableBookingTimeDTO>> GetAvailableBookingTimesInDepartment(string username);

    Task<bool> UpdateAvailableBookingToTaken(AvailableBookingTimeDTO bookingTimeDTO);

    Task<BookingMachineProgramDTO> GetBookingMachineProgramFromBooking(BookingDTO bookingDTO);

    Task<bool> InsertNewRfidCard(RfidCardDTO rfidCardDTO);

    Task<bool> RfidCardExists(string rfidCardId);

    Task<RfidCardDTO> GetRfidCard(string rfidCardId);

    Task<MasterArduinoDTO> GetMasterArduino(string? masterArduinoId, string? apiKey);

    Task<List<BookingDTO>> GetBookedBookingBasedOnRfidCard(RfidCardDTO rfidCardDTO, DateTime scannedTime);
    Task<MasterArduinoDTO> InsertNewMasterArduino(MasterArduinoDTO masterArduinoDTO);

    Task<ArduinoMachineDTO> InsertNewArduinoMachine(ArduinoMachineDTO arduinoMachineDTO);
    Task<List<ArduinoMachineDTO>> GetMachinesByArduinoMasterId(string arduinoMasterId);
    Task<ProgramResultDTO> GetProgram(BookingDTO bookingDTO);

    Task<bool> InsertNewDepartment(DepartmentDTO newDepartment);
    Task<bool> DeleteUser(UserDTO userDTO);
    Task<bool> InsertMachineProgram(MachineProgramDTO machineProgramDTO);
    Task<bool> InsertElectricityPrice(ElectricityPriceDTO electricityPriceDTO);

    Task<bool> InsertProgram(ProgramDTO programDTO);
    Task InsertAvailableBookingTimes();
    Task<bool> DeleteElectricityPrice(ElectricityPriceDTO electricityPriceDTO);
    Task<bool> ResetAvailableBookingTime(AvailableBookingTimeDTO availableBookingTimeDTO);
    
    Task<bool> UpdateAllBookingTimesToBeAvailableInDepartment(string departmentName);
    Task<List<BookingDTO>> GetUserBookings(string username);
    Task<List<ProgramDTO>> GetMachineProgramsFromMachine(string machineManufacturer, string machineModelName, string machineType);
}