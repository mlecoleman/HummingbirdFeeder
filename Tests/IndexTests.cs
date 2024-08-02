using System;
using System.Threading.Tasks;
using HummingbirdFeeder.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using IndexPage = HummingbirdFeeder.Pages.Index;

namespace Tests;

public class IndexTests
{

    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public async Task TestCorrectNumberOfDatesGetsCalculated()
    {
        //Arrange - A feeder was changes 5 days in the past
        using var ctx = new Bunit.TestContext();
        DateTime currentDate = DateTime.Now;
        DateTime fiveDaysAgo = currentDate.AddDays(-5);
        int lastChangeDate = int.Parse(fiveDaysAgo.ToString("yyyyMMdd"));
        var feeder = new Feeder { LastChangeDate = lastChangeDate };
        var component = new IndexPage();

        // Act - Logic runs to see how many days in the past from today the feeder was changed
        await component.GetListOfDatesSinceLastChangeDate(feeder);

        // Assert - The list of days since last change shoud have 6 dates (zero based)
        var dates = component.datesSinceLastFeederChange;
        Assert.That(component.datesSinceLastFeederChange.Count, Is.EqualTo(6));
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
