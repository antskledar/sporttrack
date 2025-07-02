using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportTrack.Data;
using SportTrack.Models;

namespace SportTrack.Pages.Administrator.SportEvents
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;

        public DeleteModel(SportTrack.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SportEvent SportEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportevent = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);

            if (sportevent is not null)
            {
                SportEvent = sportevent;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sportevent = await _context.Events.FindAsync(id);
            if (sportevent != null)
            {
                SportEvent = sportevent;
                _context.Events.Remove(SportEvent);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
