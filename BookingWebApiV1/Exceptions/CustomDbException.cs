using System.Data.Common;

namespace BookingWebApiV1.Exceptions;

public class CustomDbException : Exception
{
    public DbException DbException { get; }

    public CustomDbException(DbException dbException) : base(dbException.Message)
    {
        DbException = dbException;
    }
}