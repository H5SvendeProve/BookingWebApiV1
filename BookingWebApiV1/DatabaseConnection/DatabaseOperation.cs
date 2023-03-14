using System.Data;
using System.Data.SqlClient;

namespace BookingWebApiV1.DatabaseConnection;

public abstract class DatabaseOperation : IDatabaseOperation
{
    private string ConnectionString { get; }

    protected DatabaseOperation(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public async Task<DataRow?> ExecuteStoredProcedureAndGetSingleResultAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        var dataTable = new DataTable();
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(storedProcedureName, connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddRange(parameters);

        using var dataAdapter = new SqlDataAdapter(command);

        await Task.Run(() => dataAdapter.Fill(dataTable));

        await connection.CloseAsync();

        return dataTable.Rows.Count == 0 ? null : dataTable.Rows[0];
    }

    public async Task<DataTable> ExecuteStoredProcedureGetListResultAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        var dataTable = new DataTable();
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(storedProcedureName, connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddRange(parameters);

        using var dataAdapter = new SqlDataAdapter(command);
        
        await Task.Run(() => dataAdapter.Fill(dataTable));
        
        await connection.CloseAsync();

        return dataTable;
    }

    public async Task<int> ExecuteNonQueryStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(storedProcedureName, connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddRange(parameters);

        var returnValue = await command.ExecuteNonQueryAsync();

        await connection.CloseAsync();

        return returnValue;
    }

    public async Task<DataTable> ExecuteStoredProcedureNoParameters(string storedProcedureName)
    {
        var dataTable = new DataTable();
        
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand(storedProcedureName, connection);

        command.CommandType = CommandType.StoredProcedure;

        using var dataAdapter = new SqlDataAdapter(command);

        await Task.Run(() => dataAdapter.Fill(dataTable));
        
        await connection.CloseAsync();

        return dataTable;
    }
}