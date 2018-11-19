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
    [Authorize(Roles = "reports")]

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
            List<MedOrganization> medOrganizations = null;
            string userQuery = UserManager.FindByName(User.Identity.Name).MedOrganization;
            if (User.IsInRole("control"))
            {
                medOrganizations = db.MedOrganizations.Include(x => x.Patients).ToList();

            }
            else
            {
                medOrganizations = db.MedOrganizations
                    .Where(p => p.Name == userQuery)
                    .Include(x => x.Patients)
                    .ToList();
            }

            return View(medOrganizations);
        }

        [HttpPost]
        public ActionResult Index(string query)
        {
            List<MedOrganization> medOrganizations = null;
            string userQuery = UserManager.FindByName(User.Identity.Name).MedOrganization;
            if (User.IsInRole("control"))
            {
                medOrganizations = db.MedOrganizations
                    .Where(p => p.Name.Contains(query))
                    .Include(x => x.Patients)
                    .ToList();
            }
            else
            {
                medOrganizations = db.MedOrganizations
                    .Where(p => p.Name == userQuery)
                    .Include(x => x.Patients)
                    .ToList();
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
