IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MachinePrograms]') AND type in (N'U'))
CREATE TABLE MachinePrograms (
     MachineManufacturer nvarchar(64) NOT NULL,
     ModelName nvarchar(64) NOT NULL,
     ProgramId int NOT NULL,
     CONSTRAINT FK_MachinePrograms_Machines FOREIGN KEY (MachineManufacturer, ModelName)
         REFERENCES Machines (MachineManufacturer, ModelName)
         ON DELETE CASCADE,
     CONSTRAINT FK_MachinePrograms_Programs FOREIGN KEY (ProgramId)
         REFERENCES Programs (ProgramId)
         ON DELETE CASCADE,
     CONSTRAINT PK_MachinePrograms PRIMARY KEY (MachineManufacturer, ModelName, ProgramId)
);