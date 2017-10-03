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
    public class TermsController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();

        // GET: Terms
        public ActionResult Index()
        {
            return View(db.Terms.ToList());
        }

        // GET: Terms/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = db.Terms.Find(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        // GET: Terms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Terms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Term_Id,Term_Name,Term_Start,Term_End")] Term term)
        {
            if (ModelState.IsValid)
            {
                db.Terms.Add(term);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(term);
        }

        // GET: Terms/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = db.Terms.Find(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        // POST: Terms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Term_Id,Term_Name,Term_Start,Term_End")] Term term)
        {
            if (ModelState.IsValid)
            {
                db.Entry(term).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(term);
        }

        // GET: Terms/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Term term = db.Terms.Find(id);
            if (term == null)
            {
                return HttpNotFound();
            }
            return View(term);
        }

        // POST: Terms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Term term = db.Terms.Find(id);
            db.Terms.Remove(term);
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
