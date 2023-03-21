using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Api.Mapper;

public interface IRequestMapper
{
    ArduinoMachineDTO MapRequestToDTO(CreateArduinoMachineRequest createArduinoMachineRequest);
    BookingDTO MapRequestToDTO(CreateNewBookingRequest createNewBookingRequest);
    MachineDTO MapRequestToDTO(CreateNewMachineRequest createNewMachineRequest);
    MasterArduinoDTO MapRequestToDTO(CreateMasterArduinoRequest createMasterArduinoRequest);
    RfidCardDTO MapRequestToDTO(CreateNewRfidCardRequest createNewRfidCardRequest);
    UserDTO MapRequestToDTO(CreateNewUserRequest createNewUserRequest);
}