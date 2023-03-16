IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MasterArduinos]') AND type in (N'U'))
create table MasterArduinos (

    MasterArduinoId nvarchar (36) NOT NULL,
    DepartmentName nvarchar (128) not NULL,
    ApiKey nvarchar (36) NOT NULL,
    constraint PK_MasterArduinos primary key (MasterArduinoId),
    CONSTRAINT FK_MasterArduinos_Departments FOREIGN KEY (DepartmentName)
        references  Departments (DepartmentName)
        ON DELETE CASCADE
    );