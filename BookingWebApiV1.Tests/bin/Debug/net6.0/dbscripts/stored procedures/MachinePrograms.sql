IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertMachineProgram]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertMachineProgram
    @MachineManufacturer nvarchar(64),
    @ModelName nvarchar(64),
    @ProgramName nvarchar(128),
    @ProgramRunTimeMinutes int
AS
BEGIN
    DECLARE @ProgramId int
    SELECT @ProgramId = ProgramId FROM Programs WHERE ProgramName = @ProgramName
    
    IF @ProgramId IS NULL
    BEGIN
        EXEC InsertProgram @ProgramName, @ProgramRunTimeMinutes
        SELECT @ProgramId = SCOPE_IDENTITY()
    END
    
    INSERT INTO MachinePrograms (MachineManufacturer, ModelName, ProgramId)
    VALUES (@MachineManufacturer, @ModelName, @ProgramId)
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