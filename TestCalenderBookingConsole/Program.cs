using TestCalenderBookingApp;
class Program
{
    static void Main(string[] args)
    {
        var outputMsg = "";
        var testCalenderBookingApp = new TestCalenderBooking();

        if(args.Any())
        {
            if(args.Contains("ADD"))
                outputMsg = testCalenderBookingApp.AddSlot(args);
            if(args.Contains("DELETE"))
                outputMsg = testCalenderBookingApp.DeleteSlot(args);
            if(args.Contains("FIND"))
                outputMsg = testCalenderBookingApp.FindSlot(args);
            if(args.Contains("KEEP"))
                outputMsg = testCalenderBookingApp.KeepSlot(args);   
        }
        else
            outputMsg = "Please try again!";

        if(!String.IsNullOrEmpty(outputMsg))
            Console.WriteLine(outputMsg);
    }
}