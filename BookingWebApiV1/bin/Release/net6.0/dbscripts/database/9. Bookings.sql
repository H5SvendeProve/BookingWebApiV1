IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bookings]') AND type in (N'U'))
CREATE TABLE Bookings (
  BookingId int identity (1,1) NOT NULL,
  Username nvarchar (16) NULL,
  Price decimal (19,4) NOT NULL,
  StartTime DateTime NOT NULL,
  EndTime DateTime NOT NULL,
  ProgramId int NOT NULL,
  MachineManufacturer nvarchar (64) NOT NULL,
  ModelName nvarchar (64) NOT NULL,
  CONSTRAINT PK_Bookings PRIMARY KEY (BookingId),
  CONSTRAINT FK_Users FOREIGN KEY (UserName)
      REFERENCES Users (Username)
      ON DELETE SET DEFAULT,
  CONSTRAINT FK_MachinePrograms FOREIGN KEY (MachineManufacturer, ModelName, ProgramId)
      REFERENCES MachinePrograms (MachineManufacturer, ModelName, ProgramId)
      ON DELETE NO ACTION
);
