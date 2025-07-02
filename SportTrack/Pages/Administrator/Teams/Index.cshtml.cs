using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportTrack.Data;
using SportTrack.Models;


namespace SportTrack.Pages.Administrator.Teams
{
    
    

    public class IndexModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;

        public IndexModel(SportTrack.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Team> Team { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Team = await _context.Teams
                .Include(t => t.Sport).ToListAsync();
        }
    }
}
