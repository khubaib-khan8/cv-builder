// Data/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using CVBuilderAPI.Models;

namespace CVBuilderAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User>       Users      { get; set; }   // ← NEW
        public DbSet<CV>         CVs        { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education>  Educations  { get; set; }
        public DbSet<Skill>      Skills      { get; set; }
    }
}