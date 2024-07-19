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
        public bool ShowCreate { get; set; }
        public bool EditRecord { get; set; }
        public int EditingId { get; set; }
        private FeederDataContext? _context;
        public Feeder? NewFeeder { get; set; }
        public Feeder? FeederToUpdate { get; set; }
        public List<Feeder>? MyFeeders { get; set; }
        private HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("http://api.weatherapi.com/")
        };
        public Forecastday forecastday;
        public double maxTemp;

        protected override async Task OnInitializedAsync()
        {
            ShowCreate = false;
            await ShowFeeders();
        }

        // Create
        public void ShowCreateForm()
        {
            ShowCreate = true;
            NewFeeder = new Feeder();
        }

        public async Task CreateNewFeeder()
        {
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();

            if (NewFeeder is not null)
            {
                _context?.Feeders.Add(NewFeeder);
                _context?.SaveChangesAsync();
            }
            ShowCreate = false;
            await ShowFeeders();
        }

        // Read
        public async Task ShowFeeders()
        {
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();

            if (_context is not null)
            {
                MyFeeders = await _context.Feeders.ToListAsync();
            }
        }

        // Update
        public async Task ShowEditForm(Feeder myFeeder)
        {
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            FeederToUpdate = _context.Feeders.FirstOrDefault(x => x.FeederId == myFeeder.FeederId);
            EditingId = myFeeder.FeederId;
            EditRecord = true;
        }

        public async Task UpdateFeeder()
        {
            EditRecord = false;
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                if (FeederToUpdate is not null) _context.Feeders.Update(FeederToUpdate);
                await _context.SaveChangesAsync();
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

        public async Task<List<string>> GetListOfDatesSinceLastChangeDate(Feeder myFeeder)
        {
            List <string> datesSinceLastFeederChange = new List<string>();

            string lastChangeDate = (myFeeder.LastChangeDate).ToString();
            DateTime changeDate = DateTime.ParseExact(lastChangeDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime today = (DateTime.Now.Date);
            string formattedDate = today.ToString("yyyy-MM-dd");

            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                for (DateTime date = changeDate; date <= today; date = date.AddDays(1))
                {
                    string dateString = date.ToString("yyyy-MM-dd");
                    datesSinceLastFeederChange.Add(dateString);
                }
            }

            return datesSinceLastFeederChange;
        }

        public async Task<List<double>> GetListOfTemperatureMaxPerDate(Feeder myFeeder)
        {
            string zipcode = (myFeeder.Zipcode).ToString();
            int lastChangeDate = myFeeder.LastChangeDate;
            Task<List<string>> datesSinceLastFeederChange = GetListOfDatesSinceLastChangeDate(myFeeder);
            string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
            string key = "e44d36a439384f149d9182816241307";
            List<double> maxTemperaturesPerDay = new List<double>();
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                foreach (string date in await datesSinceLastFeederChange)
                {
                    string urlSuffix = $"v1/history.json?key={key}&q={zipcode}&dt={date}";
                    var response = await _client.GetAsync(urlSuffix);
                    var rawJson = await response.Content.ReadAsStringAsync();
                    Root rootObject = JsonSerializer.Deserialize<Root>(rawJson);
                    maxTemp = rootObject.forecast.forecastday[0].day.maxtemp_f;
                    maxTemperaturesPerDay.Add(maxTemp);
                }
            }
            return maxTemperaturesPerDay;
        }

        public async Task<bool> DoesFeederNeedToBeChanged(Feeder myFeeder)
        {
            Task<List<double>> dailyHighTemps = GetListOfTemperatureMaxPerDate(myFeeder);
            double maxTemp = (await dailyHighTemps).Max();
            int daysSinceChange = (await dailyHighTemps).Count();
            bool changeFeeder;

            if (maxTemp <= 70 && daysSinceChange >= 7) changeFeeder = true;
            else if (maxTemp > 70 && maxTemp <= 75 && daysSinceChange >= 6) changeFeeder = true;
            else if (maxTemp > 75 && maxTemp <= 80 && daysSinceChange >= 5) changeFeeder = true;
            else if (maxTemp > 80 && maxTemp <= 84 && daysSinceChange >= 4) changeFeeder = true;
            else if (maxTemp > 84 && maxTemp <= 88 && daysSinceChange >= 3) changeFeeder = true;
            else if (maxTemp > 88 && maxTemp <= 92 && daysSinceChange >= 2) changeFeeder = true;
            else if (maxTemp > 92 && daysSinceChange >= 1) changeFeeder = true;
            else changeFeeder = false;

            return changeFeeder;
        }
    }
}

