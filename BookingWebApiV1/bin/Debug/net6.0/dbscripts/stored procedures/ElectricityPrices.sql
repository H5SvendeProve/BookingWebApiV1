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
ALTER Procedure [dbo].[GetElectricityPrice]

	@Username nvarchar(16)

as
begin

	set NOCOUNT on;

	SELECT ep.Exr, ep.TimeStart, ep.TimeEnd,
           CASE WHEN d.ZipCode > 4999 THEN ''WestDenmark''
                ELSE ''EastDenmark''
           END AS Location
    FROM Departments d
    INNER JOIN ElectricityPrices ep ON 
        (CASE WHEN d.ZipCode > 4999 THEN ''WestDenmark'' ELSE ''EastDenmark'' END) = ep.Location
	where d.DepartmentName in (select top 1 departmentname from users where username =@Username) order by TimeStart desc, TimeEnd desc

end;')
end