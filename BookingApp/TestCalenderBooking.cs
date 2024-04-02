using System.Data.SqlClient;

namespace TestCalenderBookingApp;

public class TestCalenderBooking
{
public string AddSlot(string[] args)
    {
        if(ValidateLength(args))
        {
            var date = StringFindAndConvert(args, "/");
            var time = StringFindAndConvert(args, ":");
            
            if (!String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(time) && CheckConstraints(date, time))
            {
                var sqlQuery = @"
BEGIN
   IF NOT EXISTS (SELECT *
                    FROM [dbo].[TEST_BOOKING]
                    WHERE appointment_date = @date
                    AND start_time = @time
                 )
    BEGIN
        INSERT INTO [dbo].[TEST_BOOKING] ([appointment_date] ,[start_time])
        VALUES (@date, @time)
    END
END";
                date = DateTime.Parse(date).ToString("MM/dd/yyyy");
                return ExecuteQuery(sqlQuery, date, time);
            }
            else
                return "Unable to parse date and time, please try again";
        }
        else
            return "Insufficient arguments to add slot, please try again!";  
    }

    public string DeleteSlot(string[] args)
    {
        if(ValidateLength(args))
        {
            var date = StringFindAndConvert(args, "/");
            var time = StringFindAndConvert(args, ":");
            
            if (!String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(time))
            {
                var sqlQuery = @"
DELETE FROM 
    [dbo].[TEST_BOOKING] 
WHERE    
    appointment_date = @date
    AND start_time = @time;";
                date = DateTime.Parse(date).ToString("MM/dd/yyyy");
                return ExecuteQuery(sqlQuery, date, time);
            }
            else
                return "Unable to pares date and time, please try again";
        }
        else
            return "Insufficient arguments to add slot, please try again!";
    }

    public string FindSlot(string[] args)
    {
        if(ValidateLength(args))
            {
                var date = StringFindAndConvert(args, "/");

                if (!String.IsNullOrEmpty(date))
                {
                    var sqlQuery = @"
SELECT *
FROM [dbo].[TEST_BOOKING]
WHERE appointment_date = @date;";
                    date = DateTime.Parse(date).ToString("MM/dd/yyyy");
                    ExecuteQuery(sqlQuery, date);
                }
            }
        
        return "";
    }

    public string KeepSlot(string[] args)
    {
        if(ValidateLength(args))
            args.ToList().ForEach(i => Console.WriteLine(i.ToString()));

        return "";
    }

    public string? StringFindAndConvert (string[] args, string regex)
    {
        var result = Array.Find(args, i => i.Contains(regex));
        
        if(String.IsNullOrEmpty(result))
            return null;
        
        try
        {
            if(regex == "/")
                return DateTime.Parse(result).ToShortDateString();

            if(regex == ":")
            {
                return DateTime.Parse(result).ToShortTimeString();
            }
        }
        catch (Exception ex)
        {
            return "Exception" + ex;
        }

        return null;       
    }

    public bool ValidateLength(string[] args)
    {
        if((args.Contains("ADD") || args.Contains("DELETE")) && args.Length == 3)
            return true;

        if((args.Contains("FIND") || args.Contains("KEEP")) && args.Length == 2)
            return true;

        return false;    
    }

    public bool CheckConstraints(string date, string time)
    {
        var isValid = true;

        TimeSpan checkTime = DateTime.Parse(time).TimeOfDay;
        isValid = new TimeSpan(8, 59, 59) < checkTime && checkTime < new TimeSpan(17, 0, 0) ? true : false; 
        
        if(!isValid)
            Console.WriteLine("Please enter time between 9am - 5pm");

        if(isValid)
        {
            var weekNumberOfMonth = GetWeekNumberOfMonth(DateTime.Parse(date));
            var dayOfWeek = (int) DateTime.Parse(date).DayOfWeek;

            Console.WriteLine("Week of Month :" + weekNumberOfMonth);
            Console.WriteLine("Day of Week :" + dayOfWeek);

            if((weekNumberOfMonth == 3 && dayOfWeek == 2) && new TimeSpan(16, 0, 0) <= checkTime)
            {
                Console.WriteLine("This slot is reserved, try diferent date and time.");
                isValid = false;
            }
        }
        
        return isValid;
    }

    public int GetWeekNumberOfMonth(DateTime date)
    {
        date = date.Date;
        DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
        DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
        if (firstMonthMonday > date)
        {
            firstMonthDay = firstMonthDay.AddMonths(-1);
            firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
        }
        return (date - firstMonthMonday).Days / 7 + 1;
    }

    public string ExecuteQuery (string sqlQuery, string date, string time)
    {
        var result = "";

        using (SqlConnection connection = GetSqlConnection())
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@time", time);
                    result = command.ExecuteNonQuery() == 1 ? "Success!" : "No records modified.";
                }
                connection.Close();
            }
            catch (SqlException e)
            {
                result = "Exception: " + e.Message;
            }
        }

        return result;
    }

    public void ExecuteQuery (string sqlQuery, string date)
    {
        using (SqlConnection connection = GetSqlConnection())
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    List<string> bookedSlots = new List<string>();
                    command.Parameters.AddWithValue("@date", date);
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        bookedSlots.Add(String.Format("{0}", reader[1]));
                    }

                    var timeSlot = new TimeSpan(8, 30, 0);

                    while(!timeSlot.Equals(new TimeSpan(16, 30, 0)))
                    {
                        timeSlot = timeSlot.Add(new TimeSpan(0, 30, 0));
                        
                        if(!bookedSlots.Contains(timeSlot.ToString()))
                            Console.WriteLine(timeSlot);
                    }
                }
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }

    public SqlConnection GetSqlConnection ()
    {
        string connString = @"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True";
        return new SqlConnection(connString);
    }
}
