using System;
using HummingbirdFeeder.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace HummingbirdFeeder.Pages.CRUD
{
    public partial class Create
    {
        private static int dateInt = Today();
        [Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private FeederDataContext? _context { get; set; }
        private Feeder? NewFeeder = new Feeder { FeederName = "Feeder Name", Zipcode = "90210", LastChangeDate = dateInt };

        private static int Today()
        {
            DateTime today = DateTime.Now;
            string dateString = today.ToString("yyyyMMdd");
            int dateInt = Int32.Parse(dateString);
            return dateInt;
        }

        private async Task CreateFeeder()
        {
            _context?.Feeders.AddAsync(NewFeeder);
            await _context?.SaveChangesAsync();

            navigationManager.NavigateTo("/");
        }
    }
}

