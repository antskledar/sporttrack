using Microsoft.AspNetCore.Identity;

namespace SportTrack.Models
{
    public class ApplicationUser: IdentityUser
    {
        public DateTime? BirthDate { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? ContactPhone { get; set; }
    }
}
