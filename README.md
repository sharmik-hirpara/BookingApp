# This application is Console-Based Test Calender Booking system.

## Prerequisite
Download [SQL Server](https://go.microsoft.com/fwlink/p/?linkid=2216019&clcid=0xc09&culture=en-au&country=au) and [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup), if you don't have it already.

After installing both, open SSMS and create a table using below query
```sql
CREATE TABLE TEST_BOOKING1 (
	appointment_date DATE NOT NULL,
	start_time TIME(0) NOT NULL
)
```

## Installation 
Open this solution with Visual Studio Code, then go to **TestCalenderBookingConsole** file directory using terminal

Now,
* To reserve a time slot, type **"DOTNET RUN ADD 15/05 15:00"** in terminal
+ To delete a reservation, type **"DOTNET DELETE 15/05 15:00"**
- To find all available time slots of perticular day, type **"DOTNET FIND 15/05"**
