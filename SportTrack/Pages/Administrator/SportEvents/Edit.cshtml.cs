using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportTrack.Data;
using SportTrack.Models;

namespace SportTrack.Pages.Administrator.SportEvents
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;

        public EditModel(SportTrack.Data.ApplicationDbContext context)
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

            var sportevent =  await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            if (sportevent == null)
            {
                return NotFound();
            }
            SportEvent = sportevent;
           ViewData["AwayTeamId"] = new SelectList(_context.Teams, "Id", "Id");
           ViewData["HomeTeamId"] = new SelectList(_context.Teams, "Id", "Id");
           ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SportEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SportEventExists(SportEvent.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SportEventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
