using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace HairSalonApi
{
    public class HairSalonContext : DbContext
    {
        public HairSalonContext(DbContextOptions<HairSalonContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Master> Masters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношений и ограничений
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Master)
                .WithMany(m => m.Appointments)
                .HasForeignKey(a => a.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Добавляем начальные данные
            modelBuilder.Entity<Master>().HasData(
                new Master { MasterId = 1, FirstName = "Анна", Major = "Парикмахер" },
                new Master { MasterId = 2, FirstName = "Мария", Major = "Визажист" }
            );

            modelBuilder.Entity<Service>().HasData(
                new Service { ServiceId = 1, Name = "Женская стрижка", Price = 1500, Category = "Парикмахерские" },
                new Service { ServiceId = 2, Name = "Макияж", Price = 2000, Category = "Визаж" }
            );
        }
    }
}
