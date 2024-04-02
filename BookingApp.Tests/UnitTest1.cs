using TestCalenderBookingApp;

namespace BookingApp.Tests;

public class UnitTest1
{
    [Theory]
    [InlineData("19/04", "12:00", "/", "19/04/2024")]
    [InlineData("01/12", "12:00", "/", "1/12/2024")]

    public void DateValidatoinTestWhenValidDateIsEntered(string date, string time, string regex, string expectedDate)
    {
        var bookingApp = new TestCalenderBooking();
        string[] slot = {date, time};

        var actualDate = bookingApp.StringFindAndConvert(slot, regex);

        Assert.Equal(expectedDate, actualDate);
    }

    [Theory]
    [InlineData("04/19", "12:00", "/", "Exception")]
    [InlineData("1/31", "12:00", "/", "Exception")]

    public void DateValidatoinTestWhenInvalidDateIsEntered(string date, string time, string regex, string expectedDate)
    {
        var bookingApp = new TestCalenderBooking();
        string[] slot = {date, time};

        var actualDate = bookingApp.StringFindAndConvert(slot, regex);

        Assert.Contains(expectedDate, actualDate);
    }

    [Theory]
    [InlineData("19/04", "12:00", ":", "12:00 PM")]
    [InlineData("01/12", "17:00", ":", "5:00 PM")]

    public void TimeValidatoinTestWhenValidTimeIsEntered(string date, string time, string regex, string expectedDate)
    {
        var bookingApp = new TestCalenderBooking();
        string[] slot = {date, time};

        var actualTime = bookingApp.StringFindAndConvert(slot, regex);

        Assert.Equal(expectedDate, actualTime);
    }

    [Theory]
    [InlineData("19/04", "15:00", "/", ":")]
    [InlineData("01/12", "12:00", "/", ":")]
    [InlineData("23/12", "9:00", "/", ":")]

    public void CheckConstraintsWhenValidDateTimeEntered(string date, string time, string dateRegex, string timeRegex)
    {
        var bookingApp = new TestCalenderBooking();
        string[] slot = {date, time};

        var actualDate = bookingApp.StringFindAndConvert(slot, dateRegex);
        var actualTime = bookingApp.StringFindAndConvert(slot, timeRegex);

        Assert.True(bookingApp.CheckConstraints(actualDate, actualTime));
    }

    [Theory]
    [InlineData("19/07", "17:00", "/", ":")]
    [InlineData("16/04", "16:00", "/", ":")]
    [InlineData("23/12", "8:00", "/", ":")]

    public void CheckConstraintsWhenInvalidDateTimeEntered(string date, string time, string dateRegex, string timeRegex)
    {
        var bookingApp = new TestCalenderBooking();
        string[] slot = {date, time};

        var actualDate = bookingApp.StringFindAndConvert(slot, dateRegex);
        var actualTime = bookingApp.StringFindAndConvert(slot, timeRegex);

        Assert.False(bookingApp.CheckConstraints(actualDate, actualTime));
    }
}