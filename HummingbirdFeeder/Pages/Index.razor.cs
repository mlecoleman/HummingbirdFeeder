using Microsoft.EntityFrameworkCore;
using HummingbirdFeeder.Data;
using System.Text.Json;
using System.Globalization;
using HummingbirdFeeder.Models;
using Microsoft.AspNetCore.Components;

namespace HummingbirdFeeder.Pages
{
    public partial class Index
    {
        [Inject]
        private IDbContextFactory<FeederDataContext> ContextFactory { get; set; }
        private FeederDataContext? _context;
        public List<Feeder>? MyFeeders { get; set; }
        private HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("http://api.weatherapi.com/")
        };
        public Dictionary<Feeder, Dictionary<DateTime, double>> feederDictionary = new Dictionary<Feeder, Dictionary<DateTime, double>>();

        protected override async Task OnInitializedAsync()
        {
            await ShowFeeders();
            foreach (var feeder in MyFeeders)
            {
                feeder.ChangeFeeder = null;
                feederDictionary[feeder] = new Dictionary<DateTime, double>();
            }
            foreach (var feeder in MyFeeders)
            {
                string lastChangeDate = (feeder.LastChangeDate).ToString();
                DateTime changeDate = DateTime.ParseExact(lastChangeDate, "yyyyMMdd", CultureInfo.InvariantCulture);
                bool changeDateIsOlderThanOneWeek = (DateTime.Now - changeDate).TotalDays >= 7;
                await DoesFeederNeedToBeChanged(feeder, changeDateIsOlderThanOneWeek);
            }
        }

        public async Task ShowFeeders()
        {
            _context ??= await ContextFactory.CreateDbContextAsync();

            if (_context is not null)
            {
                MyFeeders = await _context.Feeders.ToListAsync();
            }
        }

        public string ConvertToDate(int dateInt)
        {
            string dateString = dateInt.ToString();
            DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
            string formattedDate = date.ToString("MM-dd-yyyy");

            return formattedDate;
        }

        // Delete
        public async Task DeleteFeeder(Feeder myFeeder)
        {
            if (myFeeder is not null) _context.Feeders.Remove(myFeeder);
            await _context.SaveChangesAsync();
            await ShowFeeders();
        }

        // API and feeeder change logic
        public async Task GetDatesSinceLastChangeDate(Feeder feeder)
        {
            string lastChangeDate = (feeder.LastChangeDate).ToString();
            DateTime changeDate = DateTime.ParseExact(lastChangeDate, "yyyyMMdd", CultureInfo.InvariantCulture);

            DateTime today = (DateTime.Now.Date);

            for (DateTime date = changeDate; date <= today; date = date.AddDays(1))
            {
                if (!feederDictionary[feeder].ContainsKey(date))
                {
                    double maxTemp = await GetTempMaxPerDayFromWeatherApi(feeder.Zipcode, date.ToString("yyyy-MM-dd"));
                    feederDictionary[feeder][date] = maxTemp;
                }
            }
        }

        public async Task<double> GetTempMaxPerDayFromWeatherApi(string zipcode, string date)
        {
            string key = "3b850edaec1f499cbc8163535242107";
            string urlSuffix = $"v1/history.json?key={key}&q={zipcode}&dt={date}";
            var response = await _client.GetAsync(urlSuffix);
            var rawJson = await response.Content.ReadAsStringAsync();
            Root rootObject = JsonSerializer.Deserialize<Root>(rawJson);
            var maxTemp = rootObject.forecast.forecastday[0].day.maxtemp_f;
            return maxTemp;
        }

        public async Task DoesFeederNeedToBeChanged(Feeder feeder, bool isChangeDateOlderThanOneWeek)
        {
            if (isChangeDateOlderThanOneWeek)
            {
                feeder.ChangeFeeder = true;
            }
            else
            {
                await GetDatesSinceLastChangeDate(feeder);

                double maxTemp = feederDictionary[feeder].Values.Max();
                int daysSinceChange = feederDictionary[feeder].Count();

                if (maxTemp <= 70 && daysSinceChange >= 7) feeder.ChangeFeeder = true;
                else if (maxTemp > 70 && maxTemp <= 75 && daysSinceChange >= 6) feeder.ChangeFeeder = true;
                else if (maxTemp > 75 && maxTemp <= 80 && daysSinceChange >= 5) feeder.ChangeFeeder = true;
                else if (maxTemp > 80 && maxTemp <= 84 && daysSinceChange >= 4) feeder.ChangeFeeder = true;
                else if (maxTemp > 84 && maxTemp <= 88 && daysSinceChange >= 3) feeder.ChangeFeeder = true;
                else if (maxTemp > 88 && maxTemp <= 92 && daysSinceChange >= 2) feeder.ChangeFeeder = true;
                else if (maxTemp > 92 && daysSinceChange >= 1) feeder.ChangeFeeder = true;
                else feeder.ChangeFeeder = false;
            }
        }

        public async Task ChangeFeederToday(Feeder feeder)
        {
            feeder.LastChangeDate = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            await _context.SaveChangesAsync();
            ResetChangeFeederLogic(feeder);
            string lastChangeDate = (feeder.LastChangeDate).ToString();
            DateTime changeDate = DateTime.ParseExact(lastChangeDate, "yyyyMMdd", CultureInfo.InvariantCulture);
            bool changeDateIsOlderThanOneWeek = (DateTime.Now - changeDate).TotalDays >= 7;
            await DoesFeederNeedToBeChanged(feeder, changeDateIsOlderThanOneWeek);
        }

        public void ResetChangeFeederLogic(Feeder feeder)
        {
            feederDictionary[feeder].Clear();
        }
    }
}