using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportTrack.Models;

namespace SportTrack.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {}
    public DbSet<Sport> Sports => Set<Sport>(); 
    public DbSet<Team> Teams => Set<Team>();    
    public DbSet<SportEvent> Events => Set<SportEvent>();
    public DbSet<UserFavorite> UserFavorites => Set<UserFavorite>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Event ⇄ HomeTeam
        builder.Entity<SportEvent>()
               .HasOne(e => e.HomeTeam)
               .WithMany(t => t.HomeEvents)
               .HasForeignKey(e => e.HomeTeamId)
               .OnDelete(DeleteBehavior.Restrict);

        // Event ⇄ AwayTeam
        builder.Entity<SportEvent>()
               .HasOne(e => e.AwayTeam)
               .WithMany(t => t.AwayEvents)
               .HasForeignKey(e => e.AwayTeamId)
               .OnDelete(DeleteBehavior.Restrict);

        // Event ⇄ Sport
        builder.Entity<SportEvent>()
               .HasOne(e => e.Sport)
               .WithMany(s => s.Events)
               .HasForeignKey(e => e.SportId);

        // Team ⇄ Sport (jedan sport – mnogo timova)
        builder.Entity<Team>()
               .HasOne(t => t.Sport)
               .WithMany(s => s.Teams)
               .HasForeignKey(t => t.SportId);


        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(450);
        });

        builder.Entity<IdentityRole>(entity =>
        {
            entity.Property(e => e.Id).HasMaxLength(450);
        });
    }
}
