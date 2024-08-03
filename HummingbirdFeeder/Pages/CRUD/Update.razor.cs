using System;
using System.Reflection.Emit;
using HummingbirdFeeder.Data;
using Microsoft.AspNetCore.Components;

namespace HummingbirdFeeder.Pages.CRUD
{
	public partial class Update
	{
        [Parameter] public string feederId { get; set; }
		[Inject] private NavigationManager navigationManager { get; set; }
        [Inject] private FeederDataContext? _context { get; set; }
        public Feeder? FeederToUpdate { get; set; }
        private DateTime InputLastChangeDate { get; set; } = DateTime.Now;
        private bool IsDateInFuture => InputLastChangeDate > DateTime.Now;
        private bool IsSaveDisabled => string.IsNullOrWhiteSpace(FeederToUpdate.Zipcode) || IsDateInFuture;

        protected override async Task OnParametersSetAsync()
		{
			FeederToUpdate = await _context.Feeders.FindAsync(int.Parse(feederId));
		}

        private async Task UpdateFeeder()
		{
            FeederToUpdate.LastChangeDate = ConvertToYyyymmdd(InputLastChangeDate);
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                if (FeederToUpdate is not null) _context.Feeders.Update(FeederToUpdate);
                await _context.SaveChangesAsync();
                navigationManager.NavigateTo("/");
            }
        }

        private int ConvertToYyyymmdd(DateTime date)
        {
            return int.Parse(date.ToString("yyyyMMdd"));
        }

        private void ValidateForm(ChangeEventArgs e)
        {
            StateHasChanged();
        }
    }
}

