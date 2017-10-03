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
    public class SessionTimesController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();

        // GET: SessionTimes
        public async Task<ActionResult> Index()
        {
            var sessionTimes = db.SessionTimes.Include(s => s.StudentSession).Include(s => s.TutorSession);
            return View(await sessionTimes.ToListAsync());
        }

        // GET: SessionTimes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionTime sessionTime = await db.SessionTimes.FindAsync(id);
            if (sessionTime == null)
            {
                return HttpNotFound();
            }
            return View(sessionTime);
        }

        // GET: SessionTimes/Create
        public ActionResult Create(int studentSession_id, int tutorSession_id)
        {
            var sessionTime = new SessionTime();
            sessionTime.StudentSession_Id = studentSession_id;
            sessionTime.TutorSession_Id = tutorSession_id;
            var studentSession = db.StudentSessions.Find(studentSession_id);
            var tutorSession = db.TutorSessions.Find(tutorSession_id);
            sessionTime.WaitTime_Start = studentSession.Checkin_Time;
            sessionTime.Tutoring_Start = DateTime.Now; 
            //ViewBag.StudentSession_Id = new SelectList(db.StudentSessions, "StudentSession_Id", "Student_Id");
            //ViewBag.TutorSession_Id = new SelectList(db.TutorSessions, "TutorSession_Id", "Tutor_Id");
            return View(sessionTime);
        }

        // POST: SessionTimes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SessionTime_Id,TutorSession_Id,StudentSession_Id,WaitTime_Start,Tutoring_Start,Tutoring_End")] SessionTime sessionTime)
        {
            sessionTime.Tutoring_End = DateTime.Now;
            var tutorSession = db.TutorSessions.Find(sessionTime.TutorSession_Id);
            var studentSession = db.StudentSessions.Find(sessionTime.StudentSession_Id);
            sessionTime.WaitTime_Start = studentSession.Checkin_Time;
            sessionTime.Tutoring_Start = sessionTime.Tutoring_Start;
            studentSession.Checkin_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.SessionTimes.Add(sessionTime);
               
                db.Entry(studentSession).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("StudentQueue","TutorSessions",new { tutorSession_id = tutorSession.TutorSession_Id });
            }
            
            sessionTime.WaitTime_Start = studentSession.Checkin_Time;
            sessionTime.Tutoring_Start = DateTime.Now;
            return View(sessionTime);
        }

        // GET: SessionTimes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionTime sessionTime = await db.SessionTimes.FindAsync(id);
            if (sessionTime == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentSession_Id = new SelectList(db.StudentSessions, "StudentSession_Id", "Student_Id", sessionTime.StudentSession_Id);
            ViewBag.TutorSession_Id = new SelectList(db.TutorSessions, "TutorSession_Id", "Tutor_Id", sessionTime.TutorSession_Id);
            return View(sessionTime);
        }

        // POST: SessionTimes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SessionTime_Id,TutorSession_Id,StudentSession_Id,WaitTime_Start,Tutoring_Start,Tutoring_End")] SessionTime sessionTime)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sessionTime).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.StudentSession_Id = new SelectList(db.StudentSessions, "StudentSession_Id", "Student_Id", sessionTime.StudentSession_Id);
            ViewBag.TutorSession_Id = new SelectList(db.TutorSessions, "TutorSession_Id", "Tutor_Id", sessionTime.TutorSession_Id);
            return View(sessionTime);
        }

        // GET: SessionTimes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SessionTime sessionTime = await db.SessionTimes.FindAsync(id);
            if (sessionTime == null)
            {
                return HttpNotFound();
            }
            return View(sessionTime);
        }

        // POST: SessionTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SessionTime sessionTime = await db.SessionTimes.FindAsync(id);
            db.SessionTimes.Remove(sessionTime);
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
