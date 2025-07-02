using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportTrack.Data;
using SportTrack.Models;
using Microsoft.AspNetCore.Authorization;


namespace SportTrack.Pages.Administrator.Sports
{
    [Authorize(Roles = "Admin")]

    public class DetailsModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;

        public DetailsModel(SportTrack.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Sport Sport { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sport = await _context.Sports.FirstOrDefaultAsync(m => m.Id == id);

            if (sport is not null)
            {
                Sport = sport;

                return Page();
            }

            return NotFound();
        }
    }
}
