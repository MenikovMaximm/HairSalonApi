using HairSalonApi.Models;
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
            // Настройка отношений между таблицами
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Master)
                .WithMany()
                .HasForeignKey(a => a.MasterId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany()
                .HasForeignKey(a => a.ServiceId);
        }
    }
}
