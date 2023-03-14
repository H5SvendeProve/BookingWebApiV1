using System.Data;
using BookingWebApiV1.Models.DatabaseDTOs;
using BookingWebApiV1.Models.DatabaseResultDTOs;

namespace BookingWebApiV1.Database;

public static class DataTableToDTOConverter
{
    public static List<ElectricityPriceDTO> ConvertDataTableToListOfElectricityPrice(DataTable dataTable)
    {
        var electricityPriceDTOs = new List<ElectricityPriceDTO>();

        foreach (DataRow row in dataTable.Rows)
        {
            var electricityPriceDTO = new ElectricityPriceDTO
            {
                DKKPerKWh = Convert.ToDouble(row["DKKPerKWh"]),
                EURPerKWh = Convert.ToDouble(row["EURPerKWh"]),
                Exr = Convert.ToDouble(row["Exr"]),
                TimeStart = Convert.ToDateTime(row["TimeStart"]),
                TimeEnd = Convert.ToDateTime(row["TimeEnd"]),
                Location = Convert.ToString(row["Location"])!
            };

            electricityPriceDTOs.Add(electricityPriceDTO);
        }

        return electricityPriceDTOs;
    }

    public static List<UserDTO> ConvertDataTableToListOfUsers(DataTable dataTable)
    {
        var users = new List<UserDTO>();

        foreach (DataRow row in dataTable.Rows)
        {
            var user = new UserDTO
            {
                Username = Convert.ToString(row["Username"]),
                Password = Convert.ToString(row["Password"]),
                PasswordSalt = Convert.ToString(row["PasswordSalt"]),
                UserRole = (UserRole)Enum.Parse(typeof(UserRole), Convert.ToString(row["UserRole"]))
            };

            users.Add(user);
        }

        return users;
    }

    public static int ConvertDataTableToBookingNumber(DataTable dataTable)
    {
        int bookingNumber = -1;

        foreach (DataRow row in dataTable.Rows)
        {
            bookingNumber = int.Parse(row["BookingId"].ToString());
        }

        return bookingNumber;
    }
    
    public static List<BookingElectricityPriceDTO> ConvertDataTableToBookingElectricityPriceList(DataTable dataTable)
    {
        var prices = new List<BookingElectricityPriceDTO>();

        foreach (DataRow row in dataTable.Rows)
        {
            var bookingMachineDto = new BookingElectricityPriceDTO
            {
                Location = Convert.ToString(row["Location"])!,
                DepartmentName = Convert.ToString(row["DepartmentName"])!,
                TimeEnd = Convert.ToDateTime(row["TimeEnd"]),
                TimeStart = Convert.ToDateTime(row["TimeStart"]),
                DKKPerKWh = Convert.ToDecimal(row["DKKPerKWh"])
            };

            prices.Add(bookingMachineDto);
        }

        return prices;
    }
    
    public static BookingMachineProgramDTO ConvertDataTableToBookingMachineProgramDTO(DataTable dataTable)
    {
        var bookingMachineProgramDto = new BookingMachineProgramDTO();

        foreach (DataRow row in dataTable.Rows)
        {
            bookingMachineProgramDto.MachineManufacturer = Convert.ToString(row["ProgramId"])!;
            bookingMachineProgramDto.ModelName = Convert.ToString(row["ModelName"])!;
            bookingMachineProgramDto.ProgramName = Convert.ToString(row["ProgramName"])!;
            bookingMachineProgramDto.ProgramRunTimeMinutes = Convert.ToInt16(row["ProgramRunTimeMinutes"]);
            bookingMachineProgramDto.EffectKWh = Convert.ToDecimal(row["EffectKWh"]);
        }

        return bookingMachineProgramDto;
    }

    public static RfidCardDTO ConvertDataTableToRfidCardDTO(DataTable dataTable)
    {
        var rfidCardDTO = new RfidCardDTO();

        foreach (DataRow row in dataTable.Rows)
        {
            rfidCardDTO.RfidCardId = Convert.ToString(row["RfidCardId"])!;
            rfidCardDTO.Username = Convert.ToString(row["Username"])!;
        }

        return rfidCardDTO;
    }

    public static List<BookingDTO> ConvertDataTableToBookingDTOs(DataTable dataTable)
    {
        var bookings = new List<BookingDTO>();

        foreach (DataRow row in dataTable.Rows)
        {
            var booking = new BookingDTO
            {
                BookingId = (int)row["BookingId"],
                Username = (string)row["Username"],
                Price = (decimal)row["Price"],
                StartTime = (DateTime)row["StartTime"],
                EndTime = (DateTime)row["EndTime"],
                ProgramId = (int)row["ProgramId"],
                MachineManufacturer = (string)row["MachineManufacturer"],
                ModelName = (string)row["ModelName"]
            };

            bookings.Add(booking);
        }

        return bookings;
    }

    public static MasterArduinoDTO ConvertDataRowToMasterArduinoDTO(DataRow dataRow)
    {
        return new MasterArduinoDTO
        {
            ApiKey = (string)dataRow["ApiKey"],
            DepartmentName = (string)dataRow["DepartmentName"],
            MasterArduinoId = (string)dataRow["MasterArduinoId"]
        };
    }

    public static ArduinoMachineDTO ConvertDataRowToArduinoMachineDTO(DataRow dataRow)
    {
        return new ArduinoMachineDTO
        {
            MachineManufacturer = (string)dataRow["MachineManufacturer"],
            ModelName = (string)dataRow["ModelName"],
            MasterArduinoId = (string)dataRow["MasterArduinoId"],
        };
    }

    public static ProgramResultDTO ConvertDataRowToProgramDTO(DataRow dataRow)
    {
        return new ProgramResultDTO
        {
            ProgramName = (string)dataRow["ProgramName"],
            ProgramRunTimeMinutes = (int)dataRow["ProgramRunTimeMinutes"],
            MachineManufacturer = (string)dataRow["MachineManufacturer"],
            ModelName = (string)dataRow["ModelName"]
        };
    }

    public static List<ArduinoMachineDTO> ConvertDataTableToArduinoMachineList(DataTable dataTable)
    {
        var arduinoMachines = new List<ArduinoMachineDTO>();

        foreach (DataRow row in dataTable.Rows)
        {
            var arduinoMachine = new ArduinoMachineDTO
            {
                MachineManufacturer = (string)row["MachineManufacturer"],
                ModelName = (string)row["ModelName"],
                MasterArduinoId = (string)row["MasterArduinoId"],
            };

            arduinoMachines.Add(arduinoMachine);
        }

        return arduinoMachines;
    }
}