IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertDepartmentElectricityPrice]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertDepartmentElectricityPrice
    @DepartmentName nvarchar(128),
    @Exr FLOAT,
    @TimeStart DATETIME,
    @TimeEnd DATETIME,
    @Location nvarchar(32)
AS
	begin
	if not exists (select * from DepartmentElectricityPrices where DepartmentName = @DepartmentName and Exr = @Exr and TimeStart = @TimeStart and TimeEnd = @TimeEnd and Location = @Location)
BEGIN
    INSERT INTO DepartmentElectricityPrices (DepartmentName, Exr, TimeStart, TimeEnd, Location)
    VALUES (@DepartmentName, @Exr, @TimeStart, @TimeEnd, @Location);
	end;
END;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetElectricityPrice]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE Procedure GetElectricityPrice

	@Username nvarchar(16)

as
begin

	set NOCOUNT on;


	select DepartmentName, ep.DKKPerKWh, ep.TimeStart,ep.TimeEnd, ep.Location from DepartmentElectricityPrices dep
	inner join ElectricityPrices ep
	on dep.Exr = ep.Exr and dep.Location = ep.Location and dep.TimeStart = ep.TimeStart and dep.TimeEnd = ep.TimeEnd
	where dep.DepartmentName = (select top 1 departmentName from Users where Username = @Username) order by dep.TimeStart, dep.TimeEnd

end')
end