IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
create table Departments (
    
     DepartmentName nvarchar(128) NOT NULL,
     Address nvarchar (128) NOT NULL,
     City nvarchar (128) NOT NULL,
     Zipcode nvarchar (5) NOT NULL
    
     constraint PK_Departments PRIMARY key (DepartmentName)
);