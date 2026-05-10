using DDMdiplom.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DDMdiplom.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Processor> Processors { get; set; }
        public DbSet<Motherboard> Motherboards { get; set; }
        public DbSet<Memory> Memories { get; set; }
        public DbSet<GraphicsCard> GraphicsCards { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<PowerSupply> PowerSupplies { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<CpuCooler> CpuCoolers { get; set; }
        public DbSet<WaterCooler> WaterCoolers { get; set; }
        public DbSet<Models.OperatingSystem> OperatingSystems { get; set; }
        public DbSet<Models.Monitor> Monitors { get; set; }
        public DbSet<Ups> UpsDevices { get; set; }
        public DbSet<Keyboard> Keyboards { get; set; }
        public DbSet<Mouse> Mice { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
