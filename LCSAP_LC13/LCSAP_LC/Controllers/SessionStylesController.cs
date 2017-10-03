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
    public class SessionStylesController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();

        // GET: SessionStyles
        public ActionResult Index()
        {
            return View(db.SessionStyles.ToList());
        }

        // GET: SessionStyles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionStyle sessionStyle = db.SessionStyles.Find(id);
            if (sessionStyle == null)
            {
                return HttpNotFound();
            }
            return View(sessionStyle);
        }

        // GET: SessionStyles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SessionStyles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Style_Id,Style_Description")] SessionStyle sessionStyle)
        {
            if (ModelState.IsValid)
            {
                db.SessionStyles.Add(sessionStyle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sessionStyle);
        }

        // GET: SessionStyles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionStyle sessionStyle = db.SessionStyles.Find(id);
            if (sessionStyle == null)
            {
                return HttpNotFound();
            }
            return View(sessionStyle);
        }

        // POST: SessionStyles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Style_Id,Style_Description")] SessionStyle sessionStyle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sessionStyle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sessionStyle);
        }

        // GET: SessionStyles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionStyle sessionStyle = db.SessionStyles.Find(id);
            if (sessionStyle == null)
            {
                return HttpNotFound();
            }
            return View(sessionStyle);
        }

        // POST: SessionStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SessionStyle sessionStyle = db.SessionStyles.Find(id);
            db.SessionStyles.Remove(sessionStyle);
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
