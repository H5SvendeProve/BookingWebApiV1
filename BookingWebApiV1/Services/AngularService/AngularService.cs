using BookingWebApiV1.Api.Mappers;
using BookingWebApiV1.Api.Requests;
using BookingWebApiV1.Authentication;
using BookingWebApiV1.Database;
using BookingWebApiV1.Logging;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;
using ILogger = BookingWebApiV1.Logging.ILogger;

namespace BookingWebApiV1.Services.AngularService;

public class AngularService : IAngularService
{
    private IDatabaseContext DatabaseContext { get; }
    private IJwtProvider JwtProvider { get; }
    private IRequestMapper RequestMapper { get; }
    
    private ILogger Logger { get; }

    public AngularService(IDatabaseContext databaseContext, IJwtProvider jwtProvider,
        IRequestMapper requestMapper, ILogger logger)
    {
        DatabaseContext = databaseContext;
        JwtProvider = jwtProvider;
        RequestMapper = requestMapper;
        Logger = logger;
    }

    public async Task<List<ElectricityPriceDTO>> GetPrices()
    {
        var prices = await DatabaseContext.GetElectricityPrices();

        if (prices.Any())
        {
            var groupedPrices = prices.GroupBy(price => price.TimeStart.Date)
                .Select(priceGrouping => priceGrouping.OrderByDescending(p => p.TimeStart).Take(48))
                .ToList();

            prices = groupedPrices.SelectMany(price => price).OrderByDescending(price => price.TimeStart).Take(48)
                .ToList();
        }

        return prices;
    }

    public async Task<bool> CreateNewMachine(CreateNewMachineRequest createNewMachineRequest)
    {
        var machineDTO = RequestMapper.MapRequestToDTO(createNewMachineRequest);

        return await DatabaseContext.InsertNewMachine(machineDTO);
    }

    private decimal GetPrice(List<BookingElectricityPriceDTO> prices, BookingDTO bookingDTO)
    {
        // TODO handle a default price if we dont have it yet. or take the price from yesterday
        decimal potentialPrice = 0;

        foreach (var price in prices.Where(price => price.TimeStart.Date == bookingDTO.StartTime.Date)
                     .Where(price => price.TimeStart.Hour == bookingDTO.StartTime.Hour))
        {
            potentialPrice = price.DKKPerKWh;
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
        
        Logger.LogMessage(LogType.Info, $"mapped request");

        var electricityPrices = await DatabaseContext.GetElectricityPricesBasedOnBooking(bookingDTO);

        var potentialElectricityPrice = GetPrice(electricityPrices, bookingDTO);

        var bookingProgramData = await DatabaseContext.GetBookingMachineProgramFromBooking(bookingDTO);

        var effect = CalculateMachineEffect(bookingProgramData.ProgramRunTimeMinutes, bookingProgramData.EffectKWh);

        var estimatedPrice = CalculateEstimatedPrice(effect, potentialElectricityPrice);

        bookingDTO.Price = estimatedPrice;

        bookingDTO.EndTime = bookingDTO.StartTime.AddMinutes(bookingProgramData.ProgramRunTimeMinutes);
        
        Logger.LogMessage(LogType.Info, $"parsing booking to databaseContext");

        return await DatabaseContext.InsertNewBooking(bookingDTO);
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
}