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
    public async Task Test1()
    {
        //Arrange
        using var ctx = new Bunit.TestContext();
        DateTime currentDate = DateTime.Now;
        DateTime fiveDaysAgo = currentDate.AddDays(-5);
        int lastChangeDate = int.Parse(fiveDaysAgo.ToString("yyyyMMdd"));
        var feeder = new Feeder { LastChangeDate = lastChangeDate };

        var component = new IndexPage();

        await component.GetListOfDatesSinceLastChangeDate(feeder);

        var dates = component.datesSinceLastFeederChange;
        Assert.That(component.datesSinceLastFeederChange.Count, Is.EqualTo(6));
    }
}
