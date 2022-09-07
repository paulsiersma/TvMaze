using Microsoft.EntityFrameworkCore;
using TvMaze.Data.Entities;

namespace TvMaze.Data;

public class TvMazeDbContext: DbContext
{
    public TvMazeDbContext(DbContextOptions<TvMazeDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Show>()
            .HasMany(s => s.Actors).WithMany(a => a.Shows);

        modelBuilder.Entity<Actor>()
            .HasMany(a => a.Shows).WithMany(a => a.Actors);
    }

    public DbSet<Show> Shows => Set<Show>();
    public DbSet<Actor> Actors => Set<Actor>();
}