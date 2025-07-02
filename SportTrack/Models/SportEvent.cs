﻿namespace SportTrack.Models
{
    public class SportEvent
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int SportId { get; set; }
        public Sport? Sport { get; set; }

        public int HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }

        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}
