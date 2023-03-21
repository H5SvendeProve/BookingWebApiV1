IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertMachineProgram]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertMachineProgram
    @MachineManufacturer nvarchar(64),
    @ModelName nvarchar(64),
    @ProgramId int
AS
BEGIN

    if not exists(select *
                  from MachinePrograms
                  where MachineManufacturer = @MachineManufacturer
                    and ModelName = @ModelName
                    and ProgramId = @ProgramId)
        begin

            INSERT INTO MachinePrograms (MachineManufacturer, ModelName, ProgramId)
            VALUES (@MachineManufacturer, @ModelName, @ProgramId)
        end;
END;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMachineProgram]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE GetMachineProgram 

	@ProgramId int,
	@MachineManufacturer nvarchar (64),
	@ModelName nvarchar(64)

AS

begin

	set NOCOUNT ON;


	select p.ProgramId, p.ProgramName, p.ProgramRunTimeMinutes, m.MachineManufacturer, m.ModelName, m.EffectKWh
	from MachinePrograms mp
	inner join Programs p
	on mp.ProgramId = p.ProgramId
	inner join Machines m
	on mp.MachineManufacturer = m.MachineManufacturer and mp.ModelName = m.ModelName
	where p.ProgramId = @ProgramId and m.MachineManufacturer = @MachineManufacturer and m.ModelName = @Modelname

end;')
end