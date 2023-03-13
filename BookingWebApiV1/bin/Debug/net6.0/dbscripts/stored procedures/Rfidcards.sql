IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertRfidCard]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertRfidCard
    @RfidCardId nvarchar(32),
    @Username nvarchar(16)
AS
	begin
	if not exists (select * from rfidCards where RfidCardId = @RfidCardId and Username = @Username)
BEGIN
    INSERT INTO rfidCards(RfidCardId, Username)
    VALUES (@RfidCardId, @Username)
	end;
END;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRfidCard]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create PROCEDURE GetRfidCard

	@RfidCardId nvarchar(32)

as
begin

	set NOCOUNT on;
	
	select RfidCardId, Username from RfidCards where RfidCardId = @RfidCardId
end;')
end
    
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetBookingFromRfidCard]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE procedure GetBookingFromRfidCard

    @RfidCardId nvarchar(32),
    @scannedTime Datetime
as

begin

    set NOCOUNT ON;
    select b.bookingid,
           b.username,
           b.price,
           b.starttime,
           b.endtime,
           b.programid,
           b.machinemanufacturer,
           b.ModelName
    from Bookings b
             inner join Users u
                        on u.Username = b.Username
             inner join RfidCards rc
                        on u.Username = rc.Username
    where rc.RfidCardId = @RfidCardId and startTime >= DATEADD(minute, -15,
        @scannedTime)
    AND startTime <= DATEADD(minute, 15, @scannedTime)

end;')
end