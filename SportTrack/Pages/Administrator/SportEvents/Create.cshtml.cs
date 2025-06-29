using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportTrack.Data;
using SportTrack.Models;

namespace SportTrack.Pages.Administrator.SportEvents
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
        ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id");
        ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id");
        ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public SportEvent SportEvent { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Events.Add(SportEvent);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
