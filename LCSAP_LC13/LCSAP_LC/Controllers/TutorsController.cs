using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCSAP_LC.Models;
using PagedList;

namespace LCSAP_LC.Controllers
{
    public class TutorsController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();

        // GET: Tutors
        public ActionResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tutors = db.Tutors.Include(t => t.TutorType);

            if (!String.IsNullOrEmpty(searchString))
            {
                tutors = tutors.Where(s => s.Tutor_Lname.Contains(searchString)
                                       || s.Tutor_Fname.Contains(searchString));
            }
            tutors = tutors.OrderBy(o => o.Tutor_Lname);
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(tutors.ToPagedList(pageNumber, pageSize));
            //return View(tutors.ToList());
        }

        // GET: Tutors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // GET: Tutors/Create
        public ActionResult Create()
        {
            ViewBag.TutorType_Id = new SelectList(db.TutorTypes, "TutorType_Id", "TutorType_description");
            return View();
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutor_Id,Tutor_Fname,Tutor_Lname,Tutor_PayRate,Tutor_Active,TutorType_Id")] Tutor tutor)
        {
            tutor.Tutor_Id = tutor.Tutor_Id.ToUpper();
            if (ModelState.IsValid)
            {
                db.Tutors.Add(tutor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TutorType_Id = new SelectList(db.TutorTypes, "TutorType_Id", "TutorType_description", tutor.TutorType_Id);
            return View(tutor);
        }

        // GET: Tutors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            ViewBag.TutorType_Id = new SelectList(db.TutorTypes, "TutorType_Id", "TutorType_description", tutor.TutorType_Id);
            return View(tutor);
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tutor_Id,Tutor_Fname,Tutor_Lname,Tutor_PayRate,Tutor_Active,TutorType_Id")] Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TutorType_Id = new SelectList(db.TutorTypes, "TutorType_Id", "TutorType_description", tutor.TutorType_Id);
            return View(tutor);
        }

        // GET: Tutors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tutor tutor = db.Tutors.Find(id);
            db.Tutors.Remove(tutor);
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
