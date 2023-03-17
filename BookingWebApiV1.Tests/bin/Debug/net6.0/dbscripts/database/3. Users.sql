IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
CREATE TABLE Users (
    Username nvarchar(16) NOT NULL,
    Password nvarchar (1024) NOT NULL,
    PasswordSalt nvarchar (256) NOT NULL,
    UserRole nvarchar (16) NOT NULL,
    DepartmentName nvarchar (128) NOT NULL,
    constraint PK_Users PRIMARY KEY (Username),
    constraint FK_Departments FOREIGN key (DepartmentName)
       REFERENCES Departments (DepartmentName)
       on delete NO action
);