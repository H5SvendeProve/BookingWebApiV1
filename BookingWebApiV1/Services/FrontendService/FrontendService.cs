using BookingWebApiV1.Api.Mapper;
using BookingWebApiV1.Api.RequestDTOs;
using BookingWebApiV1.Authentication;
using BookingWebApiV1.Database;
using BookingWebApiV1.Exceptions;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;

namespace BookingWebApiV1.Services.FrontendService;

public class FrontendService : IFrontendService
{
    private IDatabaseContext DatabaseContext { get; }
    private IJwtProvider JwtProvider { get; }
    private IRequestMapper RequestMapper { get; }

    public FrontendService(IDatabaseContext databaseContext, IJwtProvider jwtProvider,
        IRequestMapper requestMapper)
    {
        DatabaseContext = databaseContext;
        JwtProvider = jwtProvider;
        RequestMapper = requestMapper;
    }
    
    public async Task<bool> CreateNewMachine(CreateNewMachineRequest createNewMachineRequest)
    {
        var machineDTO = RequestMapper.MapRequestToDTO(createNewMachineRequest);

        return await DatabaseContext.InsertNewMachine(machineDTO);
    }

    private static decimal GetPrice(List<ElectricityPriceDTO> prices, BookingDTO bookingDTO)
    {
        decimal potentialPrice = 0;

        foreach (var price in prices.Where(price => price.TimeStart.Date == bookingDTO.StartTime.Date)
                     .Where(price => price.TimeStart.Hour == bookingDTO.StartTime.Hour))
        {
            potentialPrice = (decimal)price.DKKPerKWh;
        }

        return potentialPrice;
    }

    private static decimal CalculateMachineEffect(int minutesToRun, decimal effectKwh)
    {
        if (minutesToRun > 0 && effectKwh > 0)
        {
            return effectKwh * ((decimal)minutesToRun / 60);
        }

        return 0;
    }

    private static decimal CalculateEstimatedPrice(decimal effect, decimal dkkPerKwh)
    {
        if (effect > 0 && dkkPerKwh > 0)
        {
            return effect * dkkPerKwh;
        }

        return 0;
    }

    public async Task<BookingDTO> CreateNewBooking(CreateNewBookingRequest createNewBookingRequest)
    {
        var bookingDTO = RequestMapper.MapRequestToDTO(createNewBookingRequest);

        var availableTimes = await DatabaseContext.GetAvailableBookingTimesInDepartment(bookingDTO.Username);

        if (!availableTimes.Any())
        {
            throw new NotFoundException("theres no available booking times left. please try again tomorrow");
        }
        
        var firstAvailableTimeWithZeroBookingId =
            availableTimes.FirstOrDefault(t => t.StartTime == bookingDTO.StartTime && t.bookingId == 0);

        if (firstAvailableTimeWithZeroBookingId == null ||
            firstAvailableTimeWithZeroBookingId.Equals(default(AvailableBookingTimeDTO)))
        {
            throw new BadRequestException($"the time {bookingDTO.StartTime} is not available");
        }

        var electricityPrices = await DatabaseContext.GetElectricityPrices(bookingDTO.Username);

        if (!electricityPrices.Any())
        {
            throw new NotFoundException(
                "theres no electricityPrices cannot determine the estimated price of the booking");
        }
        
        var electricityPriceInBookingStartTime = electricityPrices.FirstOrDefault(price => price.TimeStart.Date == bookingDTO.StartTime.Date && price.TimeStart.Hour == bookingDTO.StartTime.Hour);

        if (electricityPriceInBookingStartTime == null || electricityPriceInBookingStartTime.Equals(default(ElectricityPriceDTO)))
        {
            throw new NotFoundException("electricityPrice is not presented in database");
        }
        
        var bookingProgramData = await DatabaseContext.GetBookingMachineProgramFromBooking(bookingDTO);

        if (bookingProgramData.Equals(default(BookingMachineProgramDTO)))
        {
            throw new NotFoundException(
                $"no machines with MachineManufacturer : {bookingDTO.MachineManufacturer} and {bookingDTO.ModelName} and programId {bookingDTO.ProgramId}");
        }
        
        
        if (bookingDTO.MachineManufacturer != bookingProgramData.MachineManufacturer)
        {
            throw new NotFoundException("machine program is not presented in the database");
        }

        var potentialElectricityPrice = GetPrice(electricityPrices, bookingDTO);

        
        var effect = CalculateMachineEffect(bookingProgramData.ProgramRunTimeMinutes, bookingProgramData.EffectKWh);

        var estimatedPrice = CalculateEstimatedPrice(effect, potentialElectricityPrice);

        if (estimatedPrice == 0)
        {
            throw new BadRequestException("unable to determine the estimated price of the booking");
        }

        bookingDTO.Price = estimatedPrice;

        bookingDTO.EndTime = bookingDTO.StartTime.AddMinutes(bookingProgramData.ProgramRunTimeMinutes);
        
        var insertedBooking = await DatabaseContext.InsertNewBooking(bookingDTO);

        firstAvailableTimeWithZeroBookingId.bookingId = insertedBooking.BookingId;

        var updatedAvailableTime =
            await DatabaseContext.UpdateAvailableBookingToTaken(firstAvailableTimeWithZeroBookingId);

        if (!updatedAvailableTime)
        {
            throw new ServerErrorException("error on updating available booking time");
        }

        return insertedBooking;
    }
    
    public async Task<bool> ValidateToken(string token)
    {
        return await JwtProvider.IsTokenValid(token);
    }

    public async Task<bool> CreateNewRfidCard(CreateNewRfidCardRequest rfidCardRequest)
    {
        var rfidCardDto = RequestMapper.MapRequestToDTO(rfidCardRequest);

        return await DatabaseContext.InsertNewRfidCard(rfidCardDto);
    }

    public async Task<MasterArduinoDTO> CreateNewMasterArduino(CreateMasterArduinoRequest createMasterArduinoRequest)
    {
        var masterArduinoDTO = RequestMapper.MapRequestToDTO(createMasterArduinoRequest);

        var newApiKey = Authentication.ApiKey.ApiKeyGenerator.GenerateNewApiKey(36);

        masterArduinoDTO.ApiKey = newApiKey;

        var dbResult = await DatabaseContext.InsertNewMasterArduino(masterArduinoDTO);

        return dbResult;
    }

    public async Task<ArduinoMachineDTO> CreateNewArduinoMachine(
        CreateArduinoMachineRequest createArduinoMachineRequest)
    {
        var arduinoMachineDTO = RequestMapper.MapRequestToDTO(createArduinoMachineRequest);

        var dbResult = await DatabaseContext.InsertNewArduinoMachine(arduinoMachineDTO);

        return dbResult;
    }

    public async Task<List<ArduinoMachineDTO>> GetMachinesByArduinoMasterId(string arduinoMasterId)
    {
        var dbResult = await DatabaseContext.GetMachinesByArduinoMasterId(arduinoMasterId);

        return dbResult;
    }

    public async Task<List<AvailableBookingTimeDTO>> GetAvailableBookingTimes(string username)
    {
        var dbResult = await DatabaseContext.GetAvailableBookingTimesInDepartment(username);
        
        return dbResult;
    }

    public async Task<List<BookingDTO>> GetUserBookings(string username)
    {
        var dbResult = await DatabaseContext.GetUserBookings(username);

        return dbResult;
    }

    public async Task<List<ProgramDTO>> GetMachineProgramsFromMachine(string machineManufacturer, string machineModelName, string machineType)
    {
        List<ProgramDTO> dbResult =
            await DatabaseContext.GetMachineProgramsFromMachine(machineManufacturer, machineModelName, machineType);

        return dbResult;
    }
}