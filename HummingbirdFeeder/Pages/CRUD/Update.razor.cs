using System;
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


		protected override async Task OnParametersSetAsync()
		{
			FeederToUpdate = await _context.Feeders.FindAsync(int.Parse(feederId));
		}

        private async Task UpdateFeeder()
		{
            _context ??= await FeederDataContextFactory.CreateDbContextAsync();
            if (_context is not null)
            {
                if (FeederToUpdate is not null) _context.Feeders.Update(FeederToUpdate);
                await _context.SaveChangesAsync();
                navigationManager.NavigateTo("/");
            }
		}
    }
}

