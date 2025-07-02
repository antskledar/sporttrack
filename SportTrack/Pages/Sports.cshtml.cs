using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportTrack.Data;
using SportTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SportsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public IList<Sport> Sports { get; set; } = [];
    public IList<Team> Teams { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public int? SelectedSportId { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SportQuery { get; set; }

    public SportsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task OnGetAsync()
    {
        var query = _context.Sports.AsQueryable();

        if (!string.IsNullOrWhiteSpace(SportQuery))
        {
            query = query.Where(s => EF.Functions.Like(s.Name, $"%{SportQuery}%"));
        }

        Sports = await query.ToListAsync();

        if (SelectedSportId.HasValue)
        {
            Teams = await _context.Teams
                .Where(t => t.SportId == SelectedSportId)
                .ToListAsync();
        }
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
        return RedirectToPage(new { SelectedSportId = team?.SportId, SportQuery });
    }
}
