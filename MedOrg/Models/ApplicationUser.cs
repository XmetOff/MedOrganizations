using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MedOrg.Models
{

    public class ApplicationUser : IdentityUser
    {
        public string MedOrganization { get; set; }
        public ApplicationUser()
        {
        }
    }
}