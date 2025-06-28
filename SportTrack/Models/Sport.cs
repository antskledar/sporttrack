using Microsoft.Extensions.Logging;

namespace SportTrack.Models
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Team>? Teams { get; set; }
        public ICollection<Event>? Events { get; set; }
    }
}
