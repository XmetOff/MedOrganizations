using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedOrg.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace MedOrg.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        private ApplicationContext db = new ApplicationContext();

        // GET: Reports
        public ActionResult Index()
        {
            ApplicationUser user = UserManager.FindByName(User.Identity.Name);
            string roleId = user.Roles.First().RoleId.ToString();
            var role = RoleManager.FindById(roleId);
            string userMedOrg = user.MedOrganization;

            List<MedOrganization> medOrganizations = null; 
            if (role.Name != "admin")
            {
                medOrganizations = db.MedOrganizations.Where(p => p.Name == userMedOrg).Include(x => x.Patients).ToList();
            }
            else
            {
                medOrganizations = db.MedOrganizations.Include(x => x.Patients).ToList();
            }

            return View(medOrganizations);
        }

        [HttpPost]
        public ActionResult Index(string query)
        {

            ApplicationUser user = UserManager.FindByName(User.Identity.Name);
            string roleId = user.Roles.First().RoleId.ToString();
            var role = RoleManager.FindById(roleId);
            string userMedOrg = user.MedOrganization;

            List<MedOrganization> medOrganizations = null;
            if (role.Name != "admin")
            {
                medOrganizations = db.MedOrganizations.Where(p => p.Name == userMedOrg).Include(x => x.Patients).ToList();
            }
            else
            {
                medOrganizations = db.MedOrganizations.Where(p => p.Name.Contains(query)).Include(x => x.Patients).ToList();
            }

            return View(medOrganizations);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
