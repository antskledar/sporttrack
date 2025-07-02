using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportTrack.Data;
using SportTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SportTrack.Pages.Administrator.Teams
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
            ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Team Team { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", Team.SportId);
                return Page();
            }

            // Provjera postoji li već team s istim imenom i sportom
            var existing = await _context.Teams
                .Include(t => t.Sport)
                .FirstOrDefaultAsync(t => t.Name == Team.Name && t.SportId == Team.SportId);

            if (existing != null)
            {
                TempData["ErrorMessage"] = $"Tim već postoji! ID: {existing.Id}, Naziv: {existing.Name}, Sport: {existing.Sport.Name}";
                ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", Team.SportId);
                return Page();
            }

            _context.Teams.Add(Team);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
