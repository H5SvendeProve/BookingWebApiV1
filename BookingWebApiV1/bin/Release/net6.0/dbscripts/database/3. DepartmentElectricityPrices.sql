IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentElectricityPrices]') AND type in (N'U'))   
CREATE TABLE DepartmentElectricityPrices (
     DepartmentName nvarchar(128) NOT NULL,
     Exr FLOAT NOT NULL,
     TimeStart DATETIME NOT NULL,
     TimeEnd DATETIME NOT NULL,
     Location nvarchar(32) NOT NULL,
     CONSTRAINT PK_DepartmentElectricityPrices PRIMARY KEY (DepartmentName, Exr, TimeStart, TimeEnd, Location),
     CONSTRAINT FK_DepartmentElectricityPrices_Departments FOREIGN KEY (DepartmentName)
         REFERENCES Departments (DepartmentName)
         ON DELETE CASCADE,
     CONSTRAINT FK_DepartmentElectricityPrices_ElectricityPrices FOREIGN KEY (Exr, TimeStart, TimeEnd, Location)
         REFERENCES ElectricityPrices (Exr, TimeStart, TimeEnd, Location)
         ON DELETE CASCADE
);