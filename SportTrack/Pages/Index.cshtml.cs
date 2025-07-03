using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportTrack.Models;
using Microsoft.AspNetCore.Identity;

namespace SportTrack.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SportTrack.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(SportTrack.Data.ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<SportEvent> SportEvent { get; set; } = default!;
        public List<UserFavorite> UserFavorites { get; set; } = new();

        public async Task OnGetAsync()
        {
            SportEvent = await _context.Events
                .Include(e => e.AwayTeam)
                .Include(e => e.HomeTeam)
                .Include(e => e.Sport)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                UserFavorites = await _context.UserFavorites
                    .Where(f => f.ApplicationUserId == userId)
                    .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostToggleFavoriteAsync(string type, int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();

            var userId = _userManager.GetUserId(User);
            var favorite = await _context.UserFavorites
                .FirstOrDefaultAsync(f => f.ApplicationUserId == userId && (
                    (type == "team" && f.TeamId == id) ||
                    (type == "sport" && f.SportId == id)
                ));

            if (favorite == null)
            {
                var newFav = new UserFavorite { ApplicationUserId = userId };
                if (type == "team") newFav.TeamId = id;
                if (type == "sport") newFav.SportId = id;
                _context.UserFavorites.Add(newFav);
            }
            else
            {
                _context.UserFavorites.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
