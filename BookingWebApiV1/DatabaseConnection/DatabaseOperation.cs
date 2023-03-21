using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using BookingWebApiV1.Exceptions;

namespace BookingWebApiV1.DatabaseConnection;

public abstract class DatabaseOperation
{
    private string ConnectionString { get; }

    protected DatabaseOperation(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected async Task<DataRow?> ExecuteStoredProcedureAndGetSingleResultAsync(string storedProcedureName,
        SqlParameter[] parameters)
    {
        var dataTable = new DataTable();
        await using var connection = new SqlConnection(ConnectionString);

        try
        {
            await connection.OpenAsync();

            await using var command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var dataAdapter = new SqlDataAdapter(command);

            await Task.Run(() => dataAdapter.Fill(dataTable));

            return dataTable.Rows.Count == 0 ? null : dataTable.Rows[0];
        }
        catch (SqlException sqlException)
        {
            throw new CustomSqlException(sqlException);
        }

        catch (InvalidOperationException invalidOperationException)
        {
            throw new InvalidOperationException("the source DataTable is invalid", invalidOperationException);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    protected async Task<DataTable> ExecuteStoredProcedureGetListResultAsync(string storedProcedureName,
        SqlParameter[] parameters)
    {
        var dataTable = new DataTable();
        await using var connection = new SqlConnection(ConnectionString);
        try
        {
            await connection.OpenAsync();

            await using var command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var dataAdapter = new SqlDataAdapter(command);

            await Task.Run(() => dataAdapter.Fill(dataTable));

            return dataTable;
        }
        catch (SqlException sqlException)
        {
            throw new CustomSqlException(sqlException);
        }

        catch (InvalidOperationException invalidOperationException)
        {
            throw new InvalidOperationException("the source DataTable is invalid", invalidOperationException);
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    protected async Task<int> ExecuteNonQueryStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
    {
        int returnVal;
        await using var connection = new SqlConnection(ConnectionString);
        try
        {
            await connection.OpenAsync();
            await using var command = new SqlCommand(storedProcedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            var returnValue = await command.ExecuteNonQueryAsync();

            returnVal = returnValue;
        }
        catch (DbException dbException)
        {
            throw new CustomDbException(dbException);
        }

        finally
        {
            await connection.CloseAsync();
        }

        return returnVal;
    }

    protected async Task<DataTable> ExecuteStoredProcedureNoParameters(string storedProcedureName)
    {
        var dataTable = new DataTable();
        await using var connection = new SqlConnection(ConnectionString);
        try
        {
            await connection.OpenAsync();

            await using var command = new SqlCommand(storedProcedureName, connection);

            command.CommandType = CommandType.StoredProcedure;

            using var dataAdapter = new SqlDataAdapter(command);

            await Task.Run(() => dataAdapter.Fill(dataTable));
        }
        catch (SqlException sqlException)
        {
            throw new CustomSqlException(sqlException);
        }

        catch (InvalidOperationException invalidOperationException)
        {
            throw new InvalidOperationException("the source DataTable is invalid", invalidOperationException);
        }

        catch (Exception dbException)
        {
            Console.WriteLine(dbException);
            throw;
        }

        finally
        {
            await connection.CloseAsync();
        }

        return dataTable;
    }
}