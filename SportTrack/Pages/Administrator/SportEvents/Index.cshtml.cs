﻿using System;
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
    public class IndexModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;

        public IndexModel(SportTrack.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<SportEvent> SportEvent { get;set; } = default!;

        public async Task OnGetAsync()
        {
            SportEvent = await _context.Events
                .Include(s => s.AwayTeam)
                .Include(s => s.HomeTeam)
                .Include(s => s.Sport).ToListAsync();
        }
    }
}
