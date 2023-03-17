IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertAvailableBookingTimes]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertAvailableBookingTimes
AS
BEGIN
    DECLARE @start_time DATETIME = CONVERT(DATETIME, CONVERT(DATE, GETDATE())) + ''06:00'';
    DECLARE @end_time DATETIME = CONVERT(DATETIME, CONVERT(DATE, GETDATE())) + ''09:00'';
    DECLARE @department_name NVARCHAR(128);
    DECLARE department_cursor CURSOR FOR SELECT DepartmentName FROM Departments;

    OPEN department_cursor;
    FETCH NEXT FROM department_cursor INTO @department_name;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        WHILE @end_time < CONVERT(DATETIME, CONVERT(DATE, DATEADD(DAY, 1, GETDATE())))
        BEGIN
            IF NOT EXISTS (SELECT * FROM AvailableBookingTimes
                           WHERE StartTime = @start_time
                           AND EndTime = @end_time
                           AND DepartmentName = @department_name)
            BEGIN
                PRINT ''Inserting into '' + @department_name;

                INSERT INTO AvailableBookingTimes (StartTime, EndTime, DepartmentName)
                VALUES (@start_time, @end_time, @department_name);
            END

            SET @start_time = DATEADD(hour, 3, @start_time);
            SET @end_time = DATEADD(hour, 3, @end_time);
        END

        FETCH NEXT FROM department_cursor INTO @department_name;
        SET @start_time = CONVERT(DATETIME, CONVERT(DATE, GETDATE())) + ''06:00'';
        SET @end_time = CONVERT(DATETIME, CONVERT(DATE, GETDATE())) + ''09:00'';
    END

    CLOSE department_cursor;
    DEALLOCATE department_cursor;
END')
end;
    
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAvailableBookingTimes]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE GetAvailableBookingTimes @Username nvarchar(16)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AvailableBookingTimes.starttime,
           AvailableBookingTimes.endtime,
           AvailableBookingTimes.departmentname,
           AvailableBookingTimes.BookingId
    FROM AvailableBookingTimes
         INNER JOIN Departments ON AvailableBookingTimes.DepartmentName = Departments.DepartmentName
    WHERE Departments.DepartmentName IN (SELECT DepartmentName FROM Users WHERE Username = @Username)
      AND AvailableBookingTimes.BookingId IS NULL;
END')
END;
    
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAvailableBookingTimes]') AND type in (N'P', N'PC'))
BEGIN
EXEC('create procedure UpdateAvailableBookingTimes

        @StartTime DateTime,
        @EndTime DateTime,
        @DepartmentName nvarchar (128),
        @BookingId int

    as

    begin
        update AvailableBookingTimes set BookingId = @BookingId where StartTime = @StartTime and EndTime = @EndTime and DepartmentName = @DepartmentName
    end;
    ')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResetAvailableBookingTime]') AND type in (N'P', N'PC'))
BEGIN
EXEC('create procedure ResetAvailableBookingTime

        @StartTime DateTime,
        @EndTime DateTime,
        @DepartmentName nvarchar (128),
        @BookingId int

    as

    begin
        update AvailableBookingTimes set BookingId = @BookingId where StartTime = @StartTime and EndTime = @EndTime and DepartmentName = @DepartmentName
    end;
    ')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAllBookingTimesToBeAvailableInDepartment]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure UpdateAllBookingTimesToBeAvailableInDepartment
    @DepartmentName nvarchar(128)
    as
begin
update dbo.AvailableBookingTimes set BookingId = default where DepartmentName = @DepartmentName;
end;')
end