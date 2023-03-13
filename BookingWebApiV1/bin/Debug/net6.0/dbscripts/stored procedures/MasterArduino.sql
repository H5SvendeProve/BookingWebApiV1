IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertMasterArduino]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertMasterArduino
    @MasterArduinoId nvarchar(36),
    @DepartmentName nvarchar(128),
    @ApiKey nvarchar(36)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT * FROM MasterArduinos WHERE MasterArduinoId = @MasterArduinoId AND DepartmentName = @DepartmentName AND ApiKey = @ApiKey)
    BEGIN
        INSERT INTO MasterArduinos (MasterArduinoId, DepartmentName, ApiKey)
        VALUES (@MasterArduinoId, @DepartmentName, @ApiKey);

        SELECT * FROM MasterArduinos WHERE MasterArduinoId = @MasterArduinoId AND DepartmentName = @DepartmentName AND ApiKey = @ApiKey;
    END;
END;')
end


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMasterArduino]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure GetMasterArduino
    @MasterArduinoId nvarchar(36),
    @ApiKey nvarchar(36)
as

    set NOCOUNT on;

begin

    select * from MasterArduinos where MasterArduinoId = @MasterArduinoId and ApiKey = @ApiKey;


end;')
end