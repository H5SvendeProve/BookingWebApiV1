IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ElectricityPrices]') AND type in (N'U'))
CREATE TABLE ElectricityPrices (
   DKKPerKWh FLOAT NOT NULL,
   EURPerKWh FLOAT NOT NULL,
   Exr FLOAT NOT NULL,
   TimeStart DATETIME NOT NULL,
   TimeEnd DATETIME NOT NULL,
   Location nvarchar(32) NOT NULL
       constraint PK_ElectricityPrices PRIMARY KEY (Exr, TimeStart, TimeEnd, Location)
);