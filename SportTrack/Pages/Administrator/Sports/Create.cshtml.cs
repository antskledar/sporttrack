using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportTrack.Data;
using SportTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SportTrack.Pages.Administrator.Sports
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;

        public CreateModel(SportTrack.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Sport Sport { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Provjera postoji li već sport s istim imenom
            var existing = await _context.Sports
                .FirstOrDefaultAsync(s => s.Name == Sport.Name);

            if (existing != null)
            {
                // Postavi TempData za prikaz poruke na view-u
                TempData["ErrorMessage"] = $"Sport već postoji! ID: {existing.Id}, Naziv: {existing.Name}";
                return Page();
            }

            _context.Sports.Add(Sport);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
