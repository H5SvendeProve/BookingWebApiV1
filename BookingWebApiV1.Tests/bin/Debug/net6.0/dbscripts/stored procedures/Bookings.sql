IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertBooking]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertBooking
(
    @Username nvarchar(16),
    @Price decimal(19, 4),
    @StartTime datetime,
    @EndTime datetime,
    @ProgramId int,
    @MachineManufacturer nvarchar(64),
    @ModelName nvarchar(64)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Bookings (Username, Price, StartTime, EndTime, ProgramId, MachineManufacturer, ModelName)
    VALUES (@Username, @Price, @StartTime, @EndTime, @ProgramId, @MachineManufacturer, @ModelName);

    SELECT SCOPE_IDENTITY() AS BookingId; -- returns the auto-generated booking number
END;')
end