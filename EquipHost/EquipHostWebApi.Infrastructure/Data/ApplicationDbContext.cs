using EquipHostWebApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipHostWebApi.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Facility>().HasData(
                new Facility { Code = "F001", Name = "Factory A", StandardArea = 500.0 },
                new Facility { Code = "F002", Name = "Factory B", StandardArea = 300.0 }
            );

            modelBuilder.Entity<EquipmentType>().HasData(
                new EquipmentType { Code = "E001", Name = "Machine X", Area = 20.0 },
                new EquipmentType { Code = "E002", Name = "Machine Y", Area = 15.0 }
            );

            modelBuilder.Entity<Contract>().HasData(
                new Contract { ContractId = 1, FacilityCode = "F001", EquipmentCode = "E002", Quantity = 2 }
            );
        }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Contract> Contracts { get;set; }
    }
}
