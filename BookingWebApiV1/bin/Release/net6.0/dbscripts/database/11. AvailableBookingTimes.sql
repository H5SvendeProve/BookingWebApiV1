IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AvailableBookingTimes]') AND type in (N'U'))
create table AvailableBookingTimes
(


    StartTime      DateTime not null,
    EndTime        DateTime not null,
    DepartmentName nvarchar (128) not null,
    BookingId      int null
        constraint PK_AvailableBookingTimes primary key (StartTime, EndTime, DepartmentName)
        constraint FK_Departments_AvailableBookingTimes foreign key (DepartmentName)
        references Departments(DepartmentName)
        on delete no action
);