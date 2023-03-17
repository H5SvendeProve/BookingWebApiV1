IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Machines]') AND type in (N'U'))
CREATE TABLE Machines (
  MachineManufacturer nvarchar(64) NOT NULL,
  ModelName nvarchar(64) NOT NULL,
  EffectKWh float NOT NULL,
  MachineType nvarchar(64) NOT NULL
  CONSTRAINT PK_Machines PRIMARY KEY (MachineManufacturer, ModelName)
);