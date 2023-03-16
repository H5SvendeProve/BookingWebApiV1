using System.Data.SqlClient;

namespace BookingWebApiV1.Exceptions;

public class CustomSqlException : Exception
{
    public SqlException SqlException { get; }
    
    public CustomSqlException(SqlException sqlException) : base(sqlException.Message)
    {
        SqlException = sqlException;
    }
}