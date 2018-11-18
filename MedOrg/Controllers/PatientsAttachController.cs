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
    public class PatientsAttachController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

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

        // GET: PatientsAttach
        public ActionResult Index()
        {
            List<Patient> patients = db.Patients
                                        .Include(p => p.AttachRequest)
                                        .Include(p => p.MedOrganization)
                                        .ToList();
            List<Patient> patientsToView = new List<Patient>();

            foreach (var patient in patients)
            {
                if (patient.AttachRequest == null)
                {
                    patientsToView.Add(patient);
                }
                else if (patient.AttachRequest.Status != "В обработке")
                    patientsToView.Add(patient);
            }

            return View(patientsToView);
        }

        [HttpPost]
        public ActionResult Index(string query)
        {
            var result = from i in db.Patients.Include(p => p.AttachRequest).Include(p => p.MedOrganization)
                         where i.FirstName.Contains(query) || i.LastName.Contains(query) ||
                               i.MiddleName.Contains(query) || i.IIN.Contains(query)
                         select i;
            List<Patient> patients = result.ToList();
            List<Patient> patientsToView = new List<Patient>();

            foreach (var patient in patients)
            {
                if (patient.AttachRequest == null)
                {
                    patientsToView.Add(patient);
                }
                else if (patient.AttachRequest.Status != "В обработке")
                    patientsToView.Add(patient);
            }
            return View(patientsToView);
        }
        

        // GET: PatientsAttach/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: PatientsAttach/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AttachRequests, "Id", "Status");
            return View();
        }

        // POST: PatientsAttach/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,MiddleName,IIN")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AttachRequests, "Id", "Status", patient.Id);
            return View(patient);
        }

        // GET: PatientsAttach/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AttachRequests, "Id", "Status", patient.Id);
            return View(patient);
        }

        // POST: PatientsAttach/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,MiddleName,IIN")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AttachRequests, "Id", "Status", patient.Id);
            return View(patient);
        }

        // GET: PatientsAttach/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: PatientsAttach/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            AttachRequest request = db.AttachRequests.Find(id);
            db.Patients.Remove(patient);
            if (request != null)
            {
                db.AttachRequests.Remove(request);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult AttachRequest(int? id)
        {
            ApplicationUser user = UserManager.FindByName(User.Identity.Name);
            string roleId = user.Roles.First().RoleId.ToString();
            var role = RoleManager.FindById(roleId);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            if (role.Name != "admin")
            {
                ViewBag.MedOrg = user.MedOrganization;
                ViewBag.ReadOnly = "true";
            }
            else
            {
                ViewBag.ReadOnly = "false";
            }

            return View(patient);
        }

        [HttpPost]
        public ActionResult AttachRequest(int id, string medOrgName)
        {
            Patient patient = db.Patients.Find(id);
           
            AttachRequest attachRequest = new AttachRequest {
                Id = id,
                CreateDate = DateTime.Now,
                Status = "В обработке"
                //Patient = patient
            };

            var medOrgs = from i in db.MedOrganizations
                          where i.Name.Contains(medOrgName)
                          select i;
            if (medOrgs.ToList().Count == 0)
            {
                attachRequest.MedOrganization = new MedOrganization() { Name = medOrgName };
                db.MedOrganizations.Add(attachRequest.MedOrganization);
            }
            else
            {
                attachRequest.MedOrganization = medOrgs.First();
            }
            //patient.AttachRequest = attachRequest;
            db.AttachRequests.Add(attachRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
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
