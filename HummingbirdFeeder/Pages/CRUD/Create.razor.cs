using System.Text.Json;
using HummingbirdFeeder.Data;
using HummingbirdFeeder.Models;
using Microsoft.AspNetCore.Components;

namespace HummingbirdFeeder.Pages.CRUD
{
    public partial class Create
    {
        private DateTime InputLastChangeDate { get; set; } = DateTime.Now;
        private bool IsDateInFuture => InputLastChangeDate > DateTime.Now;
        private bool IsZipcodeValid { get; set; } = true;
        private bool IsSaveDisabled => string.IsNullOrWhiteSpace(NewFeeder.Zipcode) || IsDateInFuture || !IsZipcodeValid;
        private static int dateInt = Today();
        private HttpClient _client = new HttpClient()
        {
            BaseAddress = new Uri("http://api.weatherapi.com/")
        };
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private FeederDataContext? _context { get; set; }
        private Feeder? NewFeeder = new Feeder { FeederName = "Feeder Name", Zipcode = "90210", LastChangeDate = dateInt };
        private string ZipcodeErrorMessage { get; set; } = string.Empty;

        private static int Today()
        {
            DateTime today = DateTime.Now;
            string dateString = today.ToString("yyyyMMdd");
            int dateInt = Int32.Parse(dateString);
            return dateInt;
        }

        private async Task CreateFeeder()
        {
            NewFeeder.LastChangeDate = ConvertToYyyymmdd(InputLastChangeDate);
            _context?.Feeders.AddAsync(NewFeeder);
            await _context?.SaveChangesAsync();

            navigationManager.NavigateTo("/");
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

