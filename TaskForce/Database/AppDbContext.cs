using Microsoft.EntityFrameworkCore;
using TaskForce.Models;

namespace TaskForce.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Progetto> Progetti { get; set; }
        public DbSet<Pausa> Pause { get; set; }
        public DbSet<MacroFase> MacroFasi { get; set; }
        public DbSet<FaseProgetto> FasiProgetto { get; set; }
        public DbSet<PresaInCarico> PreseInCarico { get; set; }

    }
}
