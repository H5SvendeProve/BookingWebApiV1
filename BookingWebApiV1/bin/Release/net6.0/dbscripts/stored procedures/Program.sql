IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertProgram]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertProgram
    @ProgramName nvarchar(128),
    @ProgramRunTimeMinutes int
AS
	begin
	if not exists (select * from Programs where ProgramName = @ProgramName and ProgramRunTimeMinutes = @ProgramRunTimeMinutes)
BEGIN
    INSERT INTO Programs (ProgramName, ProgramRunTimeMinutes)
    VALUES (@ProgramName, @ProgramRunTimeMinutes)
	end;
END;')
end
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetProgram]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE procedure GetProgram @ProgramId int
as
begin
    set NOCOUNT on;

select p.ProgramName, p.ProgramRunTimeMinutes, m.MachineManufacturer, m.ModelName, m.MachineType
from dbo.MachinePrograms mp
         inner join Machines m
                    on mp.MachineManufacturer = m.MachineManufacturer and mp.ModelName = m.ModelName
         inner join dbo.Programs p on mp.ProgramId = p.ProgramId
where p.ProgramId = @ProgramId;

end;')
end
