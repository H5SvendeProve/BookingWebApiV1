using BookingWebApiV1.Api.Requests;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Api.Mappers;

public interface IRequestMapper
{
    ArduinoMachineDTO MapRequestToDTO(CreateArduinoMachineRequest createArduinoMachineRequest);
    BookingDTO MapRequestToDTO(CreateNewBookingRequest createNewBookingRequest);
    MachineDTO MapRequestToDTO(CreateNewMachineRequest createNewMachineRequest);
    MasterArduinoDTO MapRequestToDTO(CreateMasterArduinoRequest createMasterArduinoRequest);
    RfidCardDTO MapRequestToDTO(CreateNewRfidCardRequest createNewRfidCardRequest);
    UserDTO MapRequestToDTO(CreateNewUserRequest createNewUserRequest);
}