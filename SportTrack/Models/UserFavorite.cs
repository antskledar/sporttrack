using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportTrack.Models
{
    public class UserFavorite
    {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = string.Empty;

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? User { get; set; }

        public int? TeamId { get; set; }
        public Team? Team { get; set; }

        public int? SportId { get; set; }
        public Sport? Sport { get; set; }
    }
}
