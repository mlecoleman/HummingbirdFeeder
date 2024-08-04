using HummingbirdFeeder.Data;
using IndexPage = HummingbirdFeeder.Pages.Index;

namespace Tests;

public class IndexTests
{

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public async Task TestChangeDateOverOneWeekReturnsTrueForChangeFeeder()
    {
        // Arrange - Change date is odler than one week
        using var ctx = new Bunit.TestContext();
        var component = new IndexPage();
        var feeder = new Feeder();
        bool isChangeDateOlderThanOneWeek = true;

        // Act - Logic runs to see if feeder needs to be changes
        await component.DoesFeederNeedToBeChanged(feeder, isChangeDateOlderThanOneWeek);

        // Assert
        Assert.That(feeder.ChangeFeeder, Is.True, "Feeder should be marked for change when change date is older than one week.");
    }

    [Test]
    public async Task TestApiReturnsValueInRealisticTempRange()
    {
        //Arrange - A feeder has a valid zip code and recent date
        using var ctx = new Bunit.TestContext();
        var component = new IndexPage();
        string zipcode = "40202";
        string date = DateTime.Now.ToString("yyyy-MM-dd");

        // Act - A request is sent to weatherapi.com
        double maxTemp = await component.GetTempMaxPerDayFromWeatherApi(zipcode, date);

        // Assert - The double returned is within possible temperatures on earth
        Assert.That(maxTemp, Is.InRange(-130, 135));
    }

    [Test]
    public void TestConvertDateMethod()
    {
        //Arrange - A an int date is in format yyyymmdd
        using var ctx = new Bunit.TestContext();
        var component = new IndexPage();
        int date = 19810904;

        // Act - logic run to convert int format to user friendly format
        string formattedDate = component.ConvertToDate(date);

        // Assert - The date is now a string in format "mm-dd-yyyy"
        Assert.That(formattedDate == "09-04-1981");
    }
}
