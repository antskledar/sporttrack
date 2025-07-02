using Microsoft.Extensions.Logging;

namespace SportTrack.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int SportId { get; set; }
        public Sport? Sport { get; set; }

        public ICollection<SportEvent>? HomeEvents { get; set; }
        public ICollection<SportEvent>? AwayEvents { get; set; }
    }
}
