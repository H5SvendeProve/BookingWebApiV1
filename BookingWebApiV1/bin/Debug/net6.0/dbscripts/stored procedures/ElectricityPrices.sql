IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertElectricityPrice]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertElectricityPrice
    @DKKPerKWh FLOAT,
    @EURPerKWh FLOAT,
    @Exr FLOAT,
    @TimeStart DATETIME,
    @TimeEnd DATETIME,
    @Location nvarchar(50)
AS
BEGIN

	if not exists (select * from ElectricityPrices where Exr = @Exr and TimeStart = @TimeStart and TimeEnd = @TimeEnd and Location = @Location)

	begin

    INSERT INTO ElectricityPrices (DKKPerKWh, EURPerKWh, Exr, TimeStart, TimeEnd, Location)
    VALUES (@DKKPerKWh, @EURPerKWh, @Exr, @TimeStart, @TimeEnd, @Location);

	end;
END;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PriceExistsInDb]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure PriceExistsInDb
    @TimeStart DATETIME,
    @TimeEnd DATETIME,
    @Location nvarchar(50)

as
    begin
    set nocount on;

    select 1 from ElectricityPrices where Location = @Location and convert(DATE, TimeStart) = convert(DATE, @TimeStart) and convert(DATE, TimeEnd) = convert(DATE, @TimeEnd)

    end;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetElectricityPrices]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE Procedure GetElectricityPrices

	@Username nvarchar(16)

as
begin

	set NOCOUNT on;


    SELECT ep.DKKPerKWh, ep.EURPerKWh, ep.Exr, ep.TimeStart, ep.TimeEnd,
           IIF(d.ZipCode > 4999, ''WestDenmark'', ''EastDenmark'') AS Location
    FROM Departments d
    INNER JOIN ElectricityPrices ep ON
        (IIF(d.ZipCode > 4999, ''WestDenmark'', ''EastDenmark'')) = ep.Location where
	d.DepartmentName = (select top 1 departmentName from Users where Username = @Username)order by ep.TimeStart desc, ep.TimeEnd desc

end;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateAvailableBookingTimesToBeAvailable]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure UpdateAvailableBookingTimesToBeAvailable

        @StartTime DateTime,
        @EndTime DateTime,
        @DepartmentName nvarchar (128),
        @BookingId int

    as

    begin
        update AvailableBookingTimes set BookingId = default where StartTime = @StartTime and EndTime = @EndTime and DepartmentName = @DepartmentName and AvailableBookingTimes.BookingId = @BookingId
    end;')
end

