using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportTrack.Data;
using SportTrack.Models;
using Microsoft.AspNetCore.Identity;

public class TeamsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeamsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IList<Team> Teams { get; set; } = new List<Team>();
    public Sport? Sport { get; set; }

    public async Task<IActionResult> OnGetAsync(int sportId, string? q)
    {
        Sport = await _context.Sports.FirstOrDefaultAsync(s => s.Id == sportId);
        if (Sport == null)
            return NotFound();

        var teamsQuery = _context.Teams.Where(t => t.SportId == sportId);

        if (!string.IsNullOrWhiteSpace(q))
        {
            teamsQuery = teamsQuery.Where(t => t.Name.Contains(q));
        }

        Teams = await teamsQuery.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAddFavoriteAsync(int teamId)
    {
        if (!User.Identity.IsAuthenticated)
            return Challenge();

        var userId = _userManager.GetUserId(User);

        var already = await _context.UserFavorites
            .AnyAsync(f => f.UserId == userId && f.TeamId == teamId);

        if (!already)
        {
            _context.UserFavorites.Add(new UserFavorite
            {
                UserId = userId,
                TeamId = teamId
            });
            await _context.SaveChangesAsync();
        }

        var team = await _context.Teams.FindAsync(teamId);
        return RedirectToPage(new { sportId = team.SportId });
    }
}
