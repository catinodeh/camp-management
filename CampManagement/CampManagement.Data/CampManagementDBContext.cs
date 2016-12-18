using System.Data.Entity;
using CampManagement.Domain.Entities;

namespace CampManagement.Data
{
    public class CampManagementDbContext : DbContext
    {
        public CampManagementDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<CampManagementDbContext>(null);
        }

        public DbSet<Camp> Camps { get; set; }
        public DbSet<Camper> Campers { get; set; }
        public DbSet<CampSetup> CampSetups { get; set; }
        public DbSet<ExtraActivity> ExtraActivities { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferEntry> OfferEntries { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<RegistrationCamperExtraActivitiy> RegistrationCamperExtraActivities { get; set; }
        public DbSet<RegistrationCamper> RegistrationCampers { get; set; }
        public DbSet<RegistrationPayment> RegistrationPayments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
