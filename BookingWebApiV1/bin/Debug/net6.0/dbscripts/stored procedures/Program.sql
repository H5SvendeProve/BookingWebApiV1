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