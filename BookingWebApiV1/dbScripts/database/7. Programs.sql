IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Programs]') AND type in (N'U'))
create table Programs (
  ProgramId int identity (1,1),
  ProgramName nvarchar (128) NOT NULL,
  ProgramRunTimeMinutes int not null,
  CONSTRAINT PK_Programs PRIMARY KEY (ProgramId)
);