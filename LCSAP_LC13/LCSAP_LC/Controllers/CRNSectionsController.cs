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
    public class CRNSectionsController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();

        // GET: CRNSections
        public ActionResult Index()
        {
            var cRNSections = db.CRNSections.Include(c => c.Course).Include(c => c.Term);
            cRNSections = cRNSections.Where(b => b.Term.Term_End >= DateTime.Now);
            return View(cRNSections.ToList());
        }

        // GET: CRNSections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRNSection cRNSection = db.CRNSections.Find(id);
            if (cRNSection == null)
            {
                return HttpNotFound();
            }
            return View(cRNSection);
        }

        // GET: CRNSections/Create
        public ActionResult Create()
        {
            ViewBag.Course_Id = new SelectList(db.Courses, "Course_Id", "Course_Title");
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "Term_Name");
            return View();
        }

        // POST: CRNSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CRN_Id,Course_Id,Term_Id")] CRNSection cRNSection)
        {
            if (ModelState.IsValid)
            {
                db.CRNSections.Add(cRNSection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Course_Id = new SelectList(db.Courses.OrderBy(t=>t.Course_Title), "Course_Id", "Course_Title", cRNSection.Course_Id);
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "Term_Name", cRNSection.Term_Id);
            return View(cRNSection);
        }

        // GET: CRNSections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRNSection cRNSection = db.CRNSections.Find(id);
            if (cRNSection == null)
            {
                return HttpNotFound();
            }
            ViewBag.Course_Id = new SelectList(db.Courses, "Course_Id", "Course_Title", cRNSection.Course_Id);
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "Term_Name", cRNSection.Term_Id);
            return View(cRNSection);
        }

        // POST: CRNSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CRN_Id,Course_Id,Term_Id")] CRNSection cRNSection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRNSection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Course_Id = new SelectList(db.Courses, "Course_Id", "Course_Title", cRNSection.Course_Id);
            ViewBag.Term_Id = new SelectList(db.Terms, "Term_Id", "Term_Name", cRNSection.Term_Id);
            return View(cRNSection);
        }

        // GET: CRNSections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRNSection cRNSection = db.CRNSections.Find(id);
            if (cRNSection == null)
            {
                return HttpNotFound();
            }
            return View(cRNSection);
        }

        // POST: CRNSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CRNSection cRNSection = db.CRNSections.Find(id);
            db.CRNSections.Remove(cRNSection);
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
