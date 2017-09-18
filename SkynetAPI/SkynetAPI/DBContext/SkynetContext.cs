using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using SkynetAPI.Models;

namespace SkynetAPI.DBContext
{
    public class SkynetContext : DbContext
    {
        public SkynetContext(DbContextOptions<SkynetContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Zone>()
                .HasMany(zone => zone.Clients);

            modelBuilder.Entity<Client>()
                .HasMany(client => client.Devices);
        }

        public DbSet<ZoneRelation> ZonesRelation { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Device> Devices { get; set; }
    }
}
