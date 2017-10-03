using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCSAP_LC.Models;

namespace LCSAP_LC.Controllers
{
    public class MessageNotesController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();

        // GET: MessageNotes
        public async Task<ActionResult> Index()
        {
            return View(await db.MessageNotes.ToListAsync());
        }

        // GET: MessageNotes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageNote messageNote = await db.MessageNotes.FindAsync(id);
            if (messageNote == null)
            {
                return HttpNotFound();
            }
            return View(messageNote);
        }

        // GET: MessageNotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MessageNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Message_Id,Message_Date,Message_Description")] MessageNote messageNote)
        {
            if (ModelState.IsValid)
            {
                db.MessageNotes.Add(messageNote);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(messageNote);
        }

        // GET: MessageNotes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageNote messageNote = await db.MessageNotes.FindAsync(id);
            if (messageNote == null)
            {
                return HttpNotFound();
            }
            return View(messageNote);
        }

        // POST: MessageNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Message_Id,Message_Date,Message_Description")] MessageNote messageNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messageNote).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(messageNote);
        }

        // GET: MessageNotes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MessageNote messageNote = await db.MessageNotes.FindAsync(id);
            if (messageNote == null)
            {
                return HttpNotFound();
            }
            return View(messageNote);
        }

        // POST: MessageNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MessageNote messageNote = await db.MessageNotes.FindAsync(id);
            db.MessageNotes.Remove(messageNote);
            await db.SaveChangesAsync();
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
