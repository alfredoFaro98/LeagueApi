using LeagueApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace LeagueApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Competition> Competitions => Set<Competition>();
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // Nome squadra unico
        b.Entity<Team>().HasIndex(t => t.Name).IsUnique();

        // Qualche indice utile per query
        b.Entity<Match>().HasIndex(m => new { m.CompetitionId, m.Year });
        b.Entity<Match>().HasOne(m => m.Home).WithMany().HasForeignKey(m => m.HomeId).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Match>().HasOne(m => m.Away).WithMany().HasForeignKey(m => m.AwayId).OnDelete(DeleteBehavior.Restrict);

        // Evita duplicati Home/Away nello stesso anno/competizione (se vuoi consentire più match, rimuovi questo unique)
        b.Entity<Match>().HasIndex(m => new { m.CompetitionId, m.Year, m.HomeId, m.AwayId }).IsUnique();
    }
}
