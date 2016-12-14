using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CampManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace CampManagement.Data
{
    public class CampManagementDbContext : DbContext
    {
        public CampManagementDbContext(DbContextOptions<CampManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Camp> Camps { get; set; }
        public DbSet<Camper> Campers { get; set; }
        public DbSet<CampSetup> CampSetups { get; set; }
        public DbSet<ExtraActivity> ExtraActivities { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<RegistrationCamper> RegistrationCampers { get; set; }
        public DbSet<RegistrationPayment> RegistrationPayments { get; set; }
    }
}
