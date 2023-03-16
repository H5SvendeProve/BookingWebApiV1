IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE InsertUser
    @Username nvarchar(16),
    @Password nvarchar(1024),
    @PasswordSalt nvarchar(256),
    @UserRole nvarchar(16),
	@DepartmentName nvarchar(128)
AS
BEGIN

	if not exists (select * from Users where Username = @Username)

	begin

    -- Insert the new user
    INSERT INTO Users (Username, Password, PasswordSalt, UserRole, DepartmentName)
    VALUES (@Username, @Password, @PasswordSalt, @UserRole, @DepartmentName);

	end

END;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetAllUsers]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
CREATE PROCEDURE GetAllUsers

AS
BEGIN
    SET NOCOUNT ON;

	begin

    select username, Password, PasswordSalt, UserRole from Users;

	end

END;')
end

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC('
create procedure DeleteUser

    @Username nvarchar (16)

	as
begin
delete from Users where Username = @Username;
end;')
end