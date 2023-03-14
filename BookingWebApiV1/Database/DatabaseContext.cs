using System.Data;
using System.Data.SqlClient;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;
using BookingWebApiV1.Utils;
using static BookingWebApiV1.Database.DataTableToDTOConverter;

namespace BookingWebApiV1.Database;

public class DatabaseContext : DatabaseConnection.DatabaseOperation, IDatabaseContext

{
    private readonly string dbScriptsPath = SystemUtil.GetRootPath + @"\dbScripts";
    
    public DatabaseContext(string connectionString) : base(connectionString)
    {
        
        CreateDatabaseIfNotExists(connectionString);
        CreateDatabaseTablesIfNotExists(connectionString);
        CreateDatabaseStoredProceduresAndTriggersIfNotExists(connectionString);
       
    }

    private static void ExecuteFiles(SqlConnection connection, string[] files)
    {
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            foreach (var script in files)
            {
                var fileContent = File.ReadAllText(script);

                if (!fileContent.Any()) continue;

                using var command = new SqlCommand(fileContent, connection);
                command.ExecuteNonQuery();
            }
        }
        finally
        {
            connection.Close();
        }
    }

    private void CreateDatabaseStoredProceduresAndTriggersIfNotExists(string connectionString)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            var storedProcedureScripts = Directory.GetFiles(dbScriptsPath + @"\stored procedures", "*.sql");

            var triggerScripts = Directory.GetFiles(dbScriptsPath + @"\Trigger", "*.sql");

            ExecuteFiles(connection, storedProcedureScripts);

            ExecuteFiles(connection, triggerScripts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        finally
        {
            connection.Close();
        }
    }

    private void CreateDatabaseIfNotExists(string connectionString)
    {
        bool dbExists = false;

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
        string dbToCreate = builder.InitialCatalog; // extract the database name from the connection string

        builder.InitialCatalog = "master";

        string masterConString = builder.ConnectionString;

        using SqlConnection connection = new SqlConnection(masterConString);
        try
        {
            string sql = $"SELECT db_id('{dbToCreate}')";

            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            var result = command.ExecuteScalar();

            if (result != null && int.TryParse(result.ToString(), out int dbId))
            {
                dbExists = true;
            }

            if (!dbExists)
            {
                CreateDatabase(dbToCreate, masterConString);
            }
        }

        catch (SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case 18456:
                    break;
            }
        }

        finally
        {
            connection.Close();
        }
    }

    private void CreateDatabase(string databaseName, string connectionString)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();

            string sql = $"CREATE DATABASE {databaseName}";

            using SqlCommand createCommand = new SqlCommand(sql, connection);

            createCommand.ExecuteNonQuery();
        }

        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            connection.Close();
        }
    }

    private void CreateDatabaseTablesIfNotExists(string connectionString)
    {
        using SqlConnection connection = new SqlConnection(connectionString);
        var databaseFiles = Directory
            .GetFiles(dbScriptsPath + @"\database", "*.sql")
            .OrderBy(file => int.Parse(Path.GetFileNameWithoutExtension(file).Split('.')[0])).ToArray();

        try
        {
            connection.Open();

            ExecuteFiles(connection, databaseFiles);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            connection.Close();
        }
    }

    private static SqlParameter[] ConvertDtoToSqlParameters<T>(T dto, string propertyToExclude)
    {
        var parameters = new List<SqlParameter>();

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(dto);
            var parameter = new SqlParameter($"@{property.Name}", value ?? DBNull.Value);

            if (property.Name != propertyToExclude)
            {
                parameters.Add(parameter);
            }
        }

        return parameters.ToArray();
    }

    private static SqlParameter[] ConvertDtoToSqlParameters<T>(T dto)
    {
        var parameters = new List<SqlParameter>();

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(dto);
            var parameter = new SqlParameter($"@{property.Name}", value ?? DBNull.Value);


            parameters.Add(parameter);
        }

        return parameters.ToArray();
    }


    public async Task<List<ElectricityPriceDTO>> GetElectricityPrices()
    {
        var dataTable = await ExecuteStoredProcedureNoParameters("GetAllElectricityPrices");

        var electricityPrices = ConvertDataTableToListOfElectricityPrice(dataTable);

        return electricityPrices;
    }

    public async Task<UserDTO> GetUserWithGivenUsername(string username)
    {
        var dataTable = await ExecuteStoredProcedureNoParameters("GetAllUsers");

        var Users = ConvertDataTableToListOfUsers(dataTable);

        return Users.FirstOrDefault(usr => usr.Username == username) ?? new UserDTO();
    }

    public async Task<bool> UserExists(string username)
    {
        var potentialUser = await GetUserWithGivenUsername(username);

        return potentialUser.Username == username;
    }

    public async Task<bool> InsertNewUser(UserDTO user)
    {
        var sqlParameters = ConvertDtoToSqlParameters(user);

        var result = await ExecuteNonQueryStoredProcedureAsync("InsertUser", sqlParameters);

        return result == 1;
    }

    public async Task<bool> InsertNewMachine(MachineDTO machineDTO)
    {
        var sqlParameters = ConvertDtoToSqlParameters(machineDTO);
        var result = await ExecuteNonQueryStoredProcedureAsync("InsertMachine", sqlParameters);

        return result == 1;
    }

    public async Task<BookingDTO> InsertNewBooking(BookingDTO bookingDTO)
    {
        var sqlParameters = ConvertDtoToSqlParameters(bookingDTO, "BookingId");

        var insertResult = await ExecuteStoredProcedureGetListResultAsync("InsertBooking", sqlParameters);

        var bookingNumber = ConvertDataTableToBookingNumber(insertResult);

        bookingDTO.BookingId = bookingNumber;

        return bookingDTO;
    }

    public async Task<BookingMachineProgramDTO> GetBookingMachineProgramFromBooking(BookingDTO bookingDTO)
    {
        SqlParameter[] sqlParameters =
        {
            new("@ProgramId", bookingDTO.ProgramId),
            new("@MachineManufacturer", bookingDTO.MachineManufacturer),
            new("@ModelName", bookingDTO.ModelName)
        };

        var result = await ExecuteStoredProcedureGetListResultAsync("GetMachineProgram ", sqlParameters);

        return ConvertDataTableToBookingMachineProgramDTO(result);
    }


    public async Task<List<BookingElectricityPriceDTO>> GetElectricityPricesBasedOnBooking(BookingDTO bookingDTO)
    {
        SqlParameter[] sqlParameters =
        {
            new("@UserName", bookingDTO.Username)
        };

        var result = await ExecuteStoredProcedureGetListResultAsync("GetElectricityPrice", sqlParameters);

        return ConvertDataTableToBookingElectricityPriceList(result);
    }

    public async Task<bool> InsertNewRfidCard(RfidCardDTO rfidCardDTO)
    {
        var sqlParameter = ConvertDtoToSqlParameters(rfidCardDTO);

        var result = await ExecuteNonQueryStoredProcedureAsync("InsertRfidCard", sqlParameter);

        return result == 1;
    }

    public async Task<bool> RfidCardExists(string rfidCardId)
    {
        SqlParameter[] sqlParameters =
        {
            new("@RfidCardId", rfidCardId)
        };

        var result = await ExecuteStoredProcedureGetListResultAsync("GetRfidCard", sqlParameters);

        var rfidCard = ConvertDataTableToRfidCardDTO(result);

        return rfidCard.RfidCardId == rfidCardId;
    }

    public async Task<RfidCardDTO> GetRfidCard(string rfidCardId)
    {
        SqlParameter[] sqlParameters =
        {
            new("@RfidCardId", rfidCardId)
        };

        var result = await ExecuteStoredProcedureGetListResultAsync("GetRfidCard", sqlParameters);

        return ConvertDataTableToRfidCardDTO(result);
    }

    public async Task<MasterArduinoDTO> GetMasterArduino(string? masterArduinoId, string? apiKey)
    {
        SqlParameter[] sqlParameters =
        {
            new("@MasterArduinoId", masterArduinoId),
            new("@ApiKey", apiKey)
        };
        
        var dbResult = await ExecuteStoredProcedureAndGetSingleResultAsync("GetMasterArduino",sqlParameters);

        return dbResult != null ? ConvertDataRowToMasterArduinoDTO(dbResult) : new MasterArduinoDTO();
    }


    public async Task<List<BookingDTO>> GetBookedBookingBasedOnRfidCard(RfidCardDTO rfidCardDTO)
    {
        SqlParameter[] sqlParameters =
        {
            new("@RfidCardId", rfidCardDTO.RfidCardId),
            new("@scannedTime", DateTime.Now)
        };


        var result = await ExecuteStoredProcedureGetListResultAsync("GetBookingFromRfidCard", sqlParameters);

        return ConvertDataTableToBookingDTOs(result);
    }

    public async Task<MasterArduinoDTO> InsertNewMasterArduino(MasterArduinoDTO masterArduinoDTO)
    {
        masterArduinoDTO.MasterArduinoId = GuidGenerator.GenerateNewGuidString();

        var sqlParameters = ConvertDtoToSqlParameters(masterArduinoDTO);

        var result = await ExecuteStoredProcedureAndGetSingleResultAsync("InsertMasterArduino", sqlParameters);

        return result != null ? ConvertDataRowToMasterArduinoDTO(result) : new MasterArduinoDTO();
    }

    public async Task<ArduinoMachineDTO> InsertNewArduinoMachine(ArduinoMachineDTO arduinoMachineDTO)
    {
        var sqlParameters = ConvertDtoToSqlParameters(arduinoMachineDTO);

        var dbResult = await ExecuteStoredProcedureAndGetSingleResultAsync("InsertArduinoMachine", sqlParameters);

        return dbResult != null ? ConvertDataRowToArduinoMachineDTO(dbResult) : new ArduinoMachineDTO();
    }

    public async Task<List<ArduinoMachineDTO>> GetMachinesByArduinoMasterId(string arduinoMasterId)
    {
        SqlParameter[] sqlParameters =
        {
            new("@arduinoMasterId", arduinoMasterId)
        };

        var dbResult = await ExecuteStoredProcedureGetListResultAsync("GetMachinesByArduinoMasterId", sqlParameters);

        return ConvertDataTableToArduinoMachineList(dbResult);
    }

    public async Task<ProgramResultDTO> GetProgram(BookingDTO bookingDTO)
    {
        SqlParameter[] sqlParameters =
        {
            new ("@ProgramId", bookingDTO.ProgramId)
        };

        var dbResult = await ExecuteStoredProcedureAndGetSingleResultAsync("GetProgram", sqlParameters);

        return dbResult != null ? ConvertDataRowToProgramDTO(dbResult) : new ProgramResultDTO();
    }
}