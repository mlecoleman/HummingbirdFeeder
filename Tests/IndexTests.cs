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

        // Assert - The list of days since last change shoud have 6 dates (zero based to 5)
        var dates = component.datesSinceLastFeederChange;
        Assert.That(component.datesSinceLastFeederChange.Count, Is.EqualTo(6));
    }

    [Test]
    public async Task TestApiReturnsValueInRealisticTempRange()
    {
        //Arrange - A feeder was changes 5 days in the past
        using var ctx = new Bunit.TestContext();
        var component = new IndexPage();
        string zipcode = "40202";
        string date = DateTime.Now.ToString("yyyy-MM-dd");

        // Act - Logic runs to see how many days in the past from today the feeder was changed
        double maxTemp = await component.GetTempMaxPerDayFromWeatherApi(zipcode, date);

        // Assert - The list of days since last change shoud have 6 dates (zero based to 5)
        Assert.That(maxTemp, Is.InRange(-130, 135));
    }

    [Test]
    public void TestConverToDateMethod()
    {
        //Arrange - A feeder was changes 5 days in the past
        using var ctx = new Bunit.TestContext();
        var component = new IndexPage();
        int date = 19810904;

        // Act - Logic runs to see how many days in the past from today the feeder was changed
        string formattedDate = component.ConvertToDate(date);

        // Assert - The list of days since last change shoud have 6 dates (zero based to 5)
        Assert.That(formattedDate == "09-04-1981");
    }
}
