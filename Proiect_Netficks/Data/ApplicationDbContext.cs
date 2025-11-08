using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proiect_Netficks.Models;

namespace Proiect_Netficks.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Abonament> Abonamente { get; set; } = null!;
        public DbSet<Gen> Genuri { get; set; } = null!;
        public DbSet<Film> Filme { get; set; } = null!;
        public DbSet<Serial> Seriale { get; set; } = null!;
        public DbSet<Episod> Episoade { get; set; } = null!;
        public DbSet<Istoric_Vizionari> IstoricVizionari { get; set; } = null!;
        public DbSet<Lista_Mea> ListaMea { get; set; } = null!;
        public DbSet<Recenzii> Recenzii { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<Abonament>()
                .HasOne(a => a.Utilizator)
                .WithMany(u => u.Abonamente)
                .HasForeignKey(a => a.Utilizator_ID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Film>()
                .HasOne(f => f.Gen)
                .WithMany(g => g.Filme)
                .HasForeignKey(f => f.Gen_ID);

            modelBuilder.Entity<Serial>()
                .HasOne(s => s.Gen)
                .WithMany(g => g.Seriale)
                .HasForeignKey(s => s.Gen_ID);

            modelBuilder.Entity<Episod>()
                .HasOne(e => e.Serial)
                .WithMany(s => s.Episoade)
                .HasForeignKey(e => e.Serial_ID);

            modelBuilder.Entity<Istoric_Vizionari>()
                .HasOne(iv => iv.Utilizator)
                .WithMany(u => u.IstoricVizionari)
                .HasForeignKey(iv => iv.Utilizator_ID);

            modelBuilder.Entity<Lista_Mea>()
                .HasOne(lm => lm.Utilizator)
                .WithMany(u => u.ListaMea)
                .HasForeignKey(lm => lm.Utilizator_ID);

            modelBuilder.Entity<Recenzii>()
                .HasOne(r => r.Utilizator)
                .WithMany(u => u.Recenzii)
                .HasForeignKey(r => r.Utilizator_ID);
        }
    }
}
