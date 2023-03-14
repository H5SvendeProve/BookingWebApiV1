using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BookingWebApiV1.DatabaseConnection;

public interface IDatabaseOperation
{
    Task<DataTable> ExecuteStoredProcedureGetListResultAsync(string storedProcedureName, SqlParameter [] parameters);
    Task<int> ExecuteNonQueryStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters);
    Task<DataTable> ExecuteStoredProcedureNoParameters(string storedProcedureName);
    Task<DataRow?> ExecuteStoredProcedureAndGetSingleResultAsync(string storedProcedureName, SqlParameter[] parameters);
}