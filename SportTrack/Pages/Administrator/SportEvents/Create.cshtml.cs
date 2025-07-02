using System;
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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SportEvent SportEvent { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // Ako je null, inicijaliziraj
            if (SportEvent == null)
                SportEvent = new SportEvent();

            ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name");

            if (SportEvent.SportId != 0)
            {
                var teams = await _context.Teams
                    .Where(t => t.SportId == SportEvent.SportId)
                    .ToListAsync();

                ViewData["HomeTeamId"] = new SelectList(teams, "Id", "Name", SportEvent.HomeTeamId);
                ViewData["AwayTeamId"] = new SelectList(teams, "Id", "Name", SportEvent.AwayTeamId);
            }
            else
            {
                ViewData["HomeTeamId"] = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewData["AwayTeamId"] = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var teams = await _context.Teams
                    .Where(t => t.SportId == SportEvent.SportId)
                    .ToListAsync();

                ViewData["HomeTeamId"] = new SelectList(teams, "Id", "Name", SportEvent.HomeTeamId);
                ViewData["AwayTeamId"] = new SelectList(teams, "Id", "Name", SportEvent.AwayTeamId);
                ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", SportEvent.SportId);
                return Page();
            }

            var teamsInSport = await _context.Teams
                .Where(t => t.SportId == SportEvent.SportId)
                .ToListAsync();

            if (teamsInSport.Count > 1 && SportEvent.HomeTeamId == SportEvent.AwayTeamId)
            {
                TempData["ErrorMessage"] = "Ne možete odabrati isti tim kao domaći i gostujući ako postoji više od jednog tima u sportu.";

                ViewData["HomeTeamId"] = new SelectList(teamsInSport, "Id", "Name", SportEvent.HomeTeamId);
                ViewData["AwayTeamId"] = new SelectList(teamsInSport, "Id", "Name", SportEvent.AwayTeamId);
                ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", SportEvent.SportId);
                return Page();
            }

            var recentEvent = await _context.Events
                .Include(e => e.HomeTeam)
                .Include(e => e.AwayTeam)
                .Include(e => e.Sport)
                .Where(e => e.SportId == SportEvent.SportId &&
                           (
                               (e.HomeTeamId == SportEvent.HomeTeamId && e.AwayTeamId == SportEvent.AwayTeamId) ||
                               (e.HomeTeamId == SportEvent.AwayTeamId && e.AwayTeamId == SportEvent.HomeTeamId)
                           )
                )
                .OrderByDescending(e => e.Date)
                .FirstOrDefaultAsync(e => Math.Abs(EF.Functions.DateDiffHour(e.Date, SportEvent.Date)) < 24);

            if (recentEvent != null)
            {
                TempData["ErrorMessage"] = $"Event već postoji unutar 24 sata! ID: {recentEvent.Id}, Datum: {recentEvent.Date}, Sport: {recentEvent.Sport.Name}, Home: {recentEvent.HomeTeam.Name}, Away: {recentEvent.AwayTeam.Name}";

                ViewData["HomeTeamId"] = new SelectList(teamsInSport, "Id", "Name", SportEvent.HomeTeamId);
                ViewData["AwayTeamId"] = new SelectList(teamsInSport, "Id", "Name", SportEvent.AwayTeamId);
                ViewData["SportId"] = new SelectList(_context.Sports, "Id", "Name", SportEvent.SportId);
                return Page();
            }

            _context.Events.Add(SportEvent);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        // AJAX handler
        public async Task<JsonResult> OnGetTeamsBySportAsync(int sportId)
        {
            var teams = await _context.Teams
                .Where(t => t.SportId == sportId)
                .Select(t => new { t.Id, t.Name })
                .ToListAsync();

            return new JsonResult(teams);
        }
    }
}
