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
    

    public class IndexModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;

        public IndexModel(SportTrack.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Sport> Sport { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Sport = await _context.Sports.ToListAsync();
        }
    }
}
