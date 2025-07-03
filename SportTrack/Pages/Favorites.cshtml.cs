using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SportTrack.Data;
using SportTrack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class FavoritesModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public FavoritesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IList<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (!User.Identity.IsAuthenticated)
            return Challenge();

        var userId = _userManager.GetUserId(User);

        Favorites = await _context.UserFavorites
    .Where(f => f.ApplicationUserId == userId)
    .Include(f => f.Team)
        .ThenInclude(t => t.Sport)
    .Include(f => f.Sport) 
    .ToListAsync();


        return Page();
    }

    public async Task<IActionResult> OnPostRemoveFavoriteAsync(int favoriteId)
    {
        var fav = await _context.UserFavorites.FindAsync(favoriteId);
        if (fav != null)
        {
            _context.UserFavorites.Remove(fav);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
