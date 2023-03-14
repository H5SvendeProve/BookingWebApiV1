IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RfidCards]') AND type in (N'U'))
create table RfidCards (
   RfidCardId nvarchar (32),
   Username nvarchar (16)
constraint PK_RfidCards primary key (RfidcardId),
   CONSTRAINT FK_Users_RfidCards FOREIGN KEY (UserName)
       REFERENCES Users (Username)
       ON DELETE CASCADE
);