IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ArduinoMachines]') AND type in (N'U'))
CREATE TABLE ArduinoMachines (
     MasterArduinoId nvarchar(36) NOT NULL,
     MachineManufacturer nvarchar(64) NOT NULL,
     ModelName nvarchar(64) NOT NULL,
     CONSTRAINT PK_ArduinoMachines PRIMARY KEY (MasterArduinoId, MachineManufacturer, ModelName),
     CONSTRAINT FK_ArduinoMachines_MasterArduinos FOREIGN KEY (MasterArduinoId)
         REFERENCES MasterArduinos (MasterArduinoId)
         ON DELETE CASCADE,
     CONSTRAINT FK_ArduinoMachines_Machines FOREIGN KEY (MachineManufacturer, ModelName)
         REFERENCES Machines (MachineManufacturer, ModelName)
         ON DELETE NO ACTION
);