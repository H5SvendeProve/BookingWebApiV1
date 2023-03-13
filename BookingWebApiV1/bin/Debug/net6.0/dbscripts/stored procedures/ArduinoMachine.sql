IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertArduinoMachine]') AND type = 'P')
BEGIN
EXEC('CREATE PROCEDURE InsertArduinoMachine
         @MasterArduinoId NVARCHAR(36),
         @MachineManufacturer NVARCHAR(64),
         @ModelName NVARCHAR(64)
            AS
            BEGIN

                SET NOCOUNT ON;

                IF NOT EXISTS(SELECT *
                              FROM ArduinoMachines
                              WHERE MasterArduinoId = @MasterArduinoId
                                AND MachineManufacturer = @MachineManufacturer
                                AND ModelName = @ModelName)
                    BEGIN
                        INSERT INTO ArduinoMachines (MasterArduinoId, MachineManufacturer, ModelName)
                        VALUES (@MasterArduinoId, @MachineManufacturer, @ModelName);

                        SELECT *
                        FROM ArduinoMachines
                        WHERE MasterArduinoId = @MasterArduinoId
                          AND MachineManufacturer = @MachineManufacturer
                          AND ModelName = @ModelName;
                    END
            END;')
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMachinesByArduinoMasterId]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure GetMachinesByArduinoMasterId

    @arduinoMasterId nvarchar(36)

as
begin
    select ma.MasterArduinoId, m.MachineManufacturer, m.ModelName from dbo.ArduinoMachines am
    inner join MasterArduinos ma on am.MasterArduinoId = ma.MasterArduinoId
    inner join Machines M on am.MachineManufacturer = M.MachineManufacturer and am.ModelName = M.ModelName
    where ma.MasterArduinoId = @arduinoMasterId;

end')
end
