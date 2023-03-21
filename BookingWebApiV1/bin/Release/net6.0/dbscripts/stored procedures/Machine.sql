IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertMachine]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE [dbo].[InsertMachine]
    @MachineManufacturer nvarchar(64),
    @ModelName nvarchar(64),
    @EffectKWh float,
    @MachineType nvarchar(64)
AS
	begin
	if not exists (select * from Machines where MachineManufacturer = @MachineManufacturer and ModelName = @ModelName)
BEGIN
    INSERT INTO Machines (MachineManufacturer, ModelName, EffectKWh, MachineType)
    VALUES (@MachineManufacturer, @ModelName, @EffectKWh, @MachineType);
	end;
END;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMachineProgramsFromMachine]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure GetMachineProgramsFromMachine
    @machineManufacturer nvarchar (64),
    @machineModelName nvarchar (64),
    @machineType nvarchar (32)

    as

begin
select p.* from Programs p
                    inner join MachinePrograms mp on p.ProgramId = mp.ProgramId
                    inner join Machines m on m.MachineManufacturer = mp.MachineManufacturer and m.ModelName = mp.ModelName
where mp.MachineManufacturer = @machineManufacturer and mp.ModelName = @machineModelName and m.MachineType = @machineType
end;')
end