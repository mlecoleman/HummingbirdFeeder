using System;
using Microsoft.EntityFrameworkCore;
using HummingbirdFeeder.Data;
using System.Text.Json;
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

        public async Task GetTemperatureMax()
        {
            string zipcode = "40204";
            string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
            string key = "";
            string urlSuffix = $"v1/history.json?key={key}&q={zipcode}&dt={todaysDate}";
            var response = await _client.GetAsync(urlSuffix);
            var rawJson = await response.Content.ReadAsStringAsync();
            Root rootObject = JsonSerializer.Deserialize<Root>(rawJson);
            maxTemp = rootObject.forecast.forecastday[0].day.maxtemp_f;
        }
    }
}

