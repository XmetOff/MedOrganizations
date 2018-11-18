using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedOrg.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("DbContext") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<AttachRequest> AttachRequests { get; set; }
        public DbSet<MedOrganization> MedOrganizations { get; set; }

    }
}