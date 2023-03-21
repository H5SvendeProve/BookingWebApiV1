using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Models.DatabaseDTOs;

namespace BookingWebApiV1.Api.Mapper;

public class RequestMapper : IRequestMapper
{
    public ArduinoMachineDTO MapRequestToDTO(CreateArduinoMachineRequest createArduinoMachineRequest)
    {
        return new ArduinoMachineDTO
        {
            MachineManufacturer = createArduinoMachineRequest.MachineManufacturer,
            ModelName = createArduinoMachineRequest.ModelName,
            MasterArduinoId = createArduinoMachineRequest.MasterArduinoId
        };
    }

    public BookingDTO MapRequestToDTO(CreateNewBookingRequest createNewBookingRequest)
    {
        return new BookingDTO
        {
            Username = createNewBookingRequest.Username,
            MachineManufacturer = createNewBookingRequest.MachineManufacturer,
            ModelName = createNewBookingRequest.ModelName,
            StartTime = createNewBookingRequest.StartTime,
            ProgramId = createNewBookingRequest.ProgramId,
        };
    }

    public MachineDTO MapRequestToDTO(CreateNewMachineRequest createNewMachineRequest)
    {
        return new MachineDTO
        {
            MachineManufacturer = createNewMachineRequest.MachineManufacturer,
            ModelName = createNewMachineRequest.ModelName,
            EffectKWh = createNewMachineRequest.Effect,
            MachineType = createNewMachineRequest.MachineType
        };
    }

    public MasterArduinoDTO MapRequestToDTO(CreateMasterArduinoRequest createMasterArduinoRequest)
    {
        return new MasterArduinoDTO
        {
            DepartmentName = createMasterArduinoRequest.DepartmentName
        };
    }

    public RfidCardDTO MapRequestToDTO(CreateNewRfidCardRequest createNewRfidCardRequest)
    {
        return new RfidCardDTO
        {
            RfidCardId = createNewRfidCardRequest.RfidCardId,
            Username = createNewRfidCardRequest.Username
        };
    }

    public UserDTO MapRequestToDTO(CreateNewUserRequest createNewUserRequest)
    {
        return new UserDTO
        {
            Username = createNewUserRequest.Username,
            UserRole = createNewUserRequest.UserRole,
            Password = createNewUserRequest.Password,
            PasswordSalt = "",
            DepartmentName = createNewUserRequest.DepartmentName
        };
    }
}