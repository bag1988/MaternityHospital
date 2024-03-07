using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;

namespace MaternityHospital.Context
{
    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Child> Child { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Active> Active { get; set; }

        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().HasData(new Gender()
            {
                Value = "male",
                Name = "Мужской"
            },
            new Gender()
            {
                Value = "female",
                Name = "Женский"
            },
            new Gender()
            {
                Value = "other",
                Name = "Другой"
            },
            new Gender()
            {
                Value = "unknown",
                Name = "Неизвестный"
            });
            modelBuilder.Entity<Active>().HasData(new Active()
            {
                Value = true,
                Name = "Да"
            },
            new Active()
            {
                Value = false,
                Name = "Нет"
            });
            modelBuilder.Entity<Child>(entity =>
            {
                entity.Property(b => b.Given).HasConversion(b => string.Join(",", b), b => b.Split(",", StringSplitOptions.None));
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
