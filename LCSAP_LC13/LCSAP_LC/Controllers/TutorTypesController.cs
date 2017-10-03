using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCSAP_LC.Models;

namespace LCSAP_LC.Controllers
{
    public class TutorTypesController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();

        // GET: TutorTypes
        public ActionResult Index()
        {
            return View(db.TutorTypes.ToList());
        }

        // GET: TutorTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorType tutorType = db.TutorTypes.Find(id);
            if (tutorType == null)
            {
                return HttpNotFound();
            }
            return View(tutorType);
        }

        // GET: TutorTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TutorTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TutorType_Id,TutorType_description")] TutorType tutorType)
        {
            if (ModelState.IsValid)
            {
                db.TutorTypes.Add(tutorType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tutorType);
        }

        // GET: TutorTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorType tutorType = db.TutorTypes.Find(id);
            if (tutorType == null)
            {
                return HttpNotFound();
            }
            return View(tutorType);
        }

        // POST: TutorTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TutorType_Id,TutorType_description")] TutorType tutorType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutorType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tutorType);
        }

        // GET: TutorTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorType tutorType = db.TutorTypes.Find(id);
            if (tutorType == null)
            {
                return HttpNotFound();
            }
            return View(tutorType);
        }

        // POST: TutorTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TutorType tutorType = db.TutorTypes.Find(id);
            db.TutorTypes.Remove(tutorType);
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
