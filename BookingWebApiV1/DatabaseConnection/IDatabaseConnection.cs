using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BookingWebApiV1.DatabaseConnection;

public interface IDatabaseConnection
{
    Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedureName, SqlParameter [] parameters);
    Task<int> ExecuteNonQueryStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters);
    Task<DataTable> ExecuteStoredProcedureNoParameters(string storedProcedureName);
    Task<DataRow?> ExecuteStoredProcedureAndGetResultAsync(string storedProcedureName, SqlParameter[] parameters);
}