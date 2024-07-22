using System;
using Microsoft.EntityFrameworkCore;
using HummingbirdFeeder.Data;
using System.Text.Json;
using System.Globalization;
using System.Linq;
using HummingbirdFeeder.Models;
using System.Diagnostics;

namespace HummingbirdFeeder.Pages
{
    public partial class Index
    {
        private FeederDataContext? _context;
        public List<Feeder>? MyFeeders { get; set; }
        private HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("http://api.weatherapi.com/")
        };
        public List<string> datesSinceLastFeederChange = new List<string>();
        public List<double> maxTemperaturesPerDay = new List<double>();


        protected override async Task OnInitializedAsync()
        {
            await ShowFeeders();
            foreach (var feeder in MyFeeders)
            {
                feeder.ChangeFeeder = null;
            }
            foreach (var feeder in MyFeeders)
            {
                await DoesFeederNeedToBeChanged(feeder);
            }
        }

        public async Task ShowFeeders()
        {
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();

            if (_context is not null)
            {
                MyFeeders = await _context.Feeders.ToListAsync();
            }
        }

        // Delete
        public async Task DeleteFeeder(Feeder myFeeder)
        {
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                if (myFeeder is not null) _context.Feeders.Remove(myFeeder);
                await _context.SaveChangesAsync();
            }
            await ShowFeeders();
        }

        // API and feeeder change logic

        public async Task GetListOfDatesSinceLastChangeDate(Feeder feeder)
        {
            string lastChangeDate = (feeder.LastChangeDate).ToString();
            DateTime changeDate = DateTime.ParseExact(lastChangeDate, "yyyyMMdd", CultureInfo.InvariantCulture);

            DateTime today = (DateTime.Now.Date);

            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                for (DateTime date = changeDate; date <= today; date = date.AddDays(1))
                {
                    string dateString = date.ToString("yyyy-MM-dd");
                    datesSinceLastFeederChange.Add(dateString);
                }
            }
        }

        public async Task GetListOfTemperatureMaxPerDate(Feeder feeder)
        {
            string zipcode = feeder.Zipcode;
            string key = "3b850edaec1f499cbc8163535242107";
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                foreach (string date in datesSinceLastFeederChange)
                {
                    string urlSuffix = $"v1/history.json?key={key}&q={zipcode}&dt={date}";
                    var response = await _client.GetAsync(urlSuffix);
                    var rawJson = await response.Content.ReadAsStringAsync();
                    Root rootObject = JsonSerializer.Deserialize<Root>(rawJson);
                    var maxTemp = rootObject.forecast.forecastday[0].day.maxtemp_f;
                    maxTemperaturesPerDay.Add(maxTemp);
                }
            }
        }

        public async Task DoesFeederNeedToBeChanged(Feeder feeder)
        {
            await GetListOfDatesSinceLastChangeDate(feeder);
            await GetListOfTemperatureMaxPerDate(feeder);

            double maxTemp = (maxTemperaturesPerDay).Max();
            int daysSinceChange = datesSinceLastFeederChange.Count();

            if (maxTemp <= 70 && daysSinceChange >= 7) feeder.ChangeFeeder = true;
            else if (maxTemp > 70 && maxTemp <= 75 && daysSinceChange >= 6) feeder.ChangeFeeder = true;
            else if (maxTemp > 75 && maxTemp <= 80 && daysSinceChange >= 5) feeder.ChangeFeeder = true;
            else if (maxTemp > 80 && maxTemp <= 84 && daysSinceChange >= 4) feeder.ChangeFeeder = true;
            else if (maxTemp > 84 && maxTemp <= 88 && daysSinceChange >= 3) feeder.ChangeFeeder = true;
            else if (maxTemp > 88 && maxTemp <= 92 && daysSinceChange >= 2) feeder.ChangeFeeder = true;
            else if (maxTemp > 92 && daysSinceChange >= 1) feeder.ChangeFeeder = true;
            else feeder.ChangeFeeder = false;

            ResetChangeFeederLogic();
        }

        public void ResetChangeFeederLogic()
        {
            maxTemperaturesPerDay.Clear();
            datesSinceLastFeederChange.Clear();
        }
    }
}

