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

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllElectricityPrices]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure GetAllElectricityPrices

as
    begin
    set nocount on;

    select DKKPerKwh, EURPerKWh, Exr, TimeStart, TimeEnd, Location from ElectricityPrices;

    end;')
end