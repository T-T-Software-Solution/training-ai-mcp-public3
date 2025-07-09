using Microsoft.EntityFrameworkCore;
using App.AppCore.Models;

namespace App.Database
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<ServiceAppointment> ServiceAppointments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Quotation> Quotations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quotation>(entity =>
            {
                entity.Property(q => q.Price).HasColumnType("decimal(18,2)");
                entity.Property(q => q.Discount).HasColumnType("decimal(18,2)");
            });
            
            modelBuilder.Entity<ServiceAppointment>()
            .HasOne(sa => sa.Vehicle)
            .WithMany(v => v.ServiceAppointments)
            .HasForeignKey(sa => sa.VehicleId)
            .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
