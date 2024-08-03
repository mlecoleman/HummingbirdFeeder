using System.Text.Json;
using HummingbirdFeeder.Data;
using HummingbirdFeeder.Models;
using Microsoft.AspNetCore.Components;

namespace HummingbirdFeeder.Pages.CRUD
{
	public partial class Update
	{
        [Parameter] public string feederId { get; set; }
		[Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private FeederDataContext? _context { get; set; }
        public Feeder? FeederToUpdate { get; set; }
        public Feeder TempFeeder { get; set; } = new Feeder();
        private bool IsZipcodeValid { get; set; } = true;
        private DateTime InputLastChangeDate { get; set; } = DateTime.Now;
        private bool IsDateInFuture => InputLastChangeDate > DateTime.Now;
        private bool IsSaveDisabled => string.IsNullOrWhiteSpace(FeederToUpdate.Zipcode) || IsDateInFuture || !IsZipcodeValid;
        private string ZipcodeErrorMessage { get; set; } = string.Empty;
        private HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("http://api.weatherapi.com/")
        };

        protected override async Task OnParametersSetAsync()
        {
            FeederToUpdate = await _context.Feeders.FindAsync(int.Parse(feederId));
            if (FeederToUpdate != null)
            {
                TempFeeder.FeederName = FeederToUpdate.FeederName;
                TempFeeder.Zipcode = FeederToUpdate.Zipcode;
                TempFeeder.LastChangeDate = FeederToUpdate.LastChangeDate;
            }
        }

        private async Task UpdateFeeder()
		{
            FeederToUpdate.LastChangeDate = ConvertToYyyymmdd(InputLastChangeDate);
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                if (FeederToUpdate is not null)
                {
                    _context.Feeders.Update(FeederToUpdate);
                }
                await _context.SaveChangesAsync();
                navigationManager.NavigateTo("/");
            }
        }

        public async Task UseApiToValidateZipcode(string zipcode, string date)
        {
            string key = "3b850edaec1f499cbc8163535242107";
            string urlSuffix = $"v1/history.json?key={key}&q={zipcode}&dt={date}";
            var response = await _client.GetAsync(urlSuffix);
            var rawJson = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonSerializer.Deserialize<WeatherApiErrorResponse>(rawJson);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest && errorResponse != null && errorResponse.error != null)
            {
                IsZipcodeValid = false;
                ZipcodeErrorMessage = errorResponse.error.message;
            }
            else
            {
                IsZipcodeValid = true;
                ZipcodeErrorMessage = string.Empty;
            }
            StateHasChanged();
        }

        private int ConvertToYyyymmdd(DateTime date)
        {
            return int.Parse(date.ToString("yyyyMMdd"));
        }

        private async void ValidateForm(ChangeEventArgs e)
        {

            if (e.Value is string zipcode && zipcode.Length == 5)
            {
                await UseApiToValidateZipcode(zipcode, DateTime.Now.ToString("yyyy-MM-dd"));
            }
            StateHasChanged();
        }
    }
}

