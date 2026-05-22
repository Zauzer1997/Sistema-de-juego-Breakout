using Microsoft.EntityFrameworkCore;
using BreakoutAPI.Models;

namespace BreakoutAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Score> Scores => Set<Score>();
}
