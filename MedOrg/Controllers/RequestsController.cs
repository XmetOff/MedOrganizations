using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedOrg.Models;

namespace MedOrg.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: Requests
        public ActionResult Index()
        {
            var attachRequests = db.AttachRequests.Include(a => a.Patient).Include(a => a.MedOrganization);
            return View(attachRequests.ToList());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachRequest attachRequest = db.AttachRequests.Where(x => x.Id == id).Include(x => x.MedOrganization).Single();
            //ViewBag.MedOrg = attachRequest.MedOrganization.Name;
            if (attachRequest == null)
            {
                return HttpNotFound();
            }
            return View(attachRequest);
        }

  

 

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachRequest attachRequest = db.AttachRequests.Find(id);
            if (attachRequest == null)
            {
                return HttpNotFound();
            }
            return View(attachRequest);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttachRequest attachRequest = db.AttachRequests.Find(id);
            db.AttachRequests.Remove(attachRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ApplyRequest(int requestId, int medOrgId)
        {
            var request = db.AttachRequests
                                .Where(x => x.Id == requestId)
                                .Include(x => x.MedOrganization)
                                .Include(x => x.Patient)
                                .Single();

            var patient = request.Patient;

            var medOrg = db.MedOrganizations
                                .Where(x => x.Id == medOrgId)
                                .Include(x => x.Patients)
                                .Single();

            medOrg.Patients.Add(patient);
            request.Status = "Прикреплен";
            request.HandleDate = DateTime.Now;
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RejectRequest(int id)
        {
            var request = db.AttachRequests.Find(id);
            request.Status = "Запрос отклонен";
            request.HandleDate = DateTime.Now;
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
