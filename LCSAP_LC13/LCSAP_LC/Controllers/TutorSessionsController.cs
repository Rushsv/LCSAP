using LCSAP_LC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LCSAP_LC.Services;
using LCSAP_LC.ViewModels;

namespace LCSAP_LC.Controllers
{
    public class TutorSessionsController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();
        private LCSAPServices lcServices = new LCSAPServices();
       
        // GET: TutorSession
        public ActionResult Index()
        {
            return RedirectToAction("TutorLogin");
        }

        public ActionResult TutorLogin(int? errorMsg)
        {
            ViewBag.errorMessage = (errorMsg == 1 ? "Id Not Found, Try Again" : "");
            return View();
        }

        [HttpPost]
        public ActionResult TutorLogin([Bind(Include = "Tutor_Id")] TutorSession loginTutor)
        {
            string id = loginTutor.Tutor_Id.ToUpper();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = db.Tutors.Find(id);
            if (tutor == null)
            {
                return RedirectToAction("TutorLogin", new {errorMsg = 1 });
            }
            return RedirectToAction("OpenSession", new { tutor_id = id });

        }

        // GET: TutorSessions/OpenSession
        public ActionResult OpenSession(string tutor_Id)
        {
            TutorSession tutorSession = new TutorSession();
            tutorSession.Tutor_Id = tutor_Id;
            ViewBag.Campus_Id = new SelectList(db.Campuses, "Campus_Id", "Campus_Name");
            ViewBag.Style_Id = new SelectList(db.SessionStyles, "Style_Id", "Style_Description");
            ViewBag.Tutor_Id = tutor_Id;
            return View(tutorSession);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OpenSession([Bind(Include = "Style_Id,Campus_Id,Tutor_Id")] TutorSession tutorSession)
        {
            var tutor = db.Tutors.Find(tutorSession.Tutor_Id);
            tutorSession.Session_Date = DateTime.Now;
            tutorSession.Session_Open = true;
            tutorSession.Session_Cost = tutor.Tutor_PayRate;
            if (ModelState.IsValid)
            {
                db.TutorSessions.Add(tutorSession);
                await db.SaveChangesAsync();
                //return RedirectToAction("StudentQueue", new { tutor_id = tutorSession.Tutor_Id });
                return RedirectToAction("StudentQueue", new { tutorsession_id = tutorSession.TutorSession_Id });
            }

            return RedirectToAction("TutorLogin");
        }

        public ActionResult StudentQueue(int tutorsession_id, int? filterArea)
        {

            ViewBag.filterArea = new SelectList(db.Areas, "Area_Id", "Area_Name");
            //var tutorSession = db.TutorSessions.OrderByDescending(d => d.Session_Date).Where(d => d.Tutor_Id == tutor_id && d.Session_Open).First();
            var tutorSession = db.TutorSessions.Find(tutorsession_id);
            int UMessages = db.Communications.Where(c => c.Tutor_Id == tutorSession.Tutor_Id && c.Message_Read == false).Count();
            ViewBag.UnreadMessages = UMessages;
            ViewBag.AreaFilter = filterArea;
            ViewBag.campus_Id = tutorSession.Campus_Id;
            if (tutorSession == null)
                return RedirectToAction("TutorLogin");
            ViewBag.Tutor_Id = tutorSession.Tutor_Id;
            ViewBag.TutorSession_Id = tutorsession_id;
            var studentSession = db.StudentSessions.Where(s => s.Campus_Id == tutorSession.Campus_Id && s.Session_Active).Include(t=>t.CRNSection).Include(c=>c.CRNSection.Course);
            DateTime ayer = DateTime.Today;
            studentSession = studentSession.Where(d => d.Checkin_Time >= ayer);

            if (filterArea >= 0)
            {
                studentSession = studentSession.Where(x => x.Area_Id == filterArea);
            }
            studentSession = studentSession.OrderBy(c => c.Checkin_Time);
            

            return View(studentSession);
        }

        // GET: TutorSessions/CreateTimes
        public ActionResult CreateTimes(int studentSession_id, int tutorSession_id)
        {
            var sessionTime = new SessionTime();
            sessionTime.StudentSession_Id = studentSession_id;
            sessionTime.TutorSession_Id = tutorSession_id;
            var studentSession = db.StudentSessions.Find(studentSession_id);
            var tutorSession = db.TutorSessions.Find(tutorSession_id);
            TempRemove(studentSession);
            sessionTime.WaitTime_Start = studentSession.Checkin_Time;
            sessionTime.Tutoring_Start = DateTime.Now;
            ViewBag.StudentFullName = String.Format("{0} {1} {2}", studentSession.Student.Student_FirstName, studentSession.Student.Student_MName, studentSession.Student.Student_LastName);
            ViewBag.CourseDescription = String.Format("{0}-{1} {2}", studentSession.CRNSection.Course_Id, studentSession.CRNSection.CRN_Id, studentSession.CRNSection.Course.Course_Title);
            ViewBag.Session_Active = false;
            return View(sessionTime);
        }


        // POST: TutorSessions/CreateTimes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTimes([Bind(Include = "SessionTime_Id,TutorSession_Id,StudentSession_Id,WaitTime_Start,Tutoring_Start,Tutoring_End")] SessionTime sessionTime)
        {
            var s_active = Request.Form["session_active"];
            bool session_active = s_active == "on" ? true : false;
            sessionTime.Tutoring_End = DateTime.Now;
            var tutorSession = db.TutorSessions.Find(sessionTime.TutorSession_Id);
            var studentSession = db.StudentSessions.Find(sessionTime.StudentSession_Id);
            studentSession.Session_Active = session_active;
            sessionTime.WaitTime_Start = studentSession.Checkin_Time;
            sessionTime.Tutoring_Start = sessionTime.Tutoring_Start;
            studentSession.Checkin_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.SessionTimes.Add(sessionTime);

                db.Entry(studentSession).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("StudentQueue", new { tutorSession_id = tutorSession.TutorSession_Id, filterArea = studentSession.Area_Id });
            }

            ViewBag.StudentFullName = String.Format("{0} {1} {2}", studentSession.Student.Student_FirstName, studentSession.Student.Student_MName, studentSession.Student.Student_LastName);
            ViewBag.CourseDescription = String.Format("{0}-{1} {2}", studentSession.CRNSection.Course_Id, studentSession.CRNSection.CRN_Id, studentSession.CRNSection.Course.Course_Title);
            sessionTime.WaitTime_Start = studentSession.Checkin_Time;
            sessionTime.Tutoring_Start = DateTime.Now;
            return View(sessionTime);
        }

        private void TempRemove(StudentSession studentSession)
        {
            studentSession.Session_Active = false;
            db.Entry(studentSession).State = EntityState.Modified;
            db.SaveChanges();
        }

        public ActionResult CheckInQueue(int studentSession_id, int tutorSession_id)
        {
            var sessionTime = new SessionTime();
            var studentSession = db.StudentSessions.Find(studentSession_id);
            ViewBag.StudentFullName = String.Format("{0} {1} {2}", studentSession.Student.Student_FirstName, studentSession.Student.Student_MName, studentSession.Student.Student_LastName);
            ViewBag.CourseDescription = String.Format("{0}-{1} {2}", studentSession.CRNSection.Course_Id, studentSession.CRNSection.CRN_Id, studentSession.CRNSection.Course.Course_Title);
            sessionTime.StudentSession_Id = studentSession_id;
            sessionTime.TutorSession_Id = tutorSession_id;
            return View(sessionTime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckInQueue([Bind(Include = "TutorSession_Id,StudentSession_Id")] SessionTime sessionTime)
        {
            var studentSession = db.StudentSessions.Find(sessionTime.StudentSession_Id);
            studentSession.Checkin_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(studentSession).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("StudentQueue", new { tutorSession_id = sessionTime.TutorSession_Id, filterArea = studentSession.Area_Id });
        }

        public ActionResult TutorLogout(int tutorSession_id)
        {
            var tutorSession = db.TutorSessions.Find(tutorSession_id);
            return View(tutorSession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TutorLogout([Bind(Include = "TutorSession_Id,Style_Id,Campus_Id,Tutor_Id")] TutorSession tutorSession, string BtnPressed)
        {
            if (BtnPressed == "Logout")
            {

                //TutorSession tutorOpenSession = db.TutorSessions.Find(tutorSession.TutorSession_Id);
                var openSessions = db.TutorSessions.Where(o => o.Tutor_Id == tutorSession.Tutor_Id && o.Session_Open == true);
                foreach (var tutorOpenSession in openSessions)
                {
                    tutorOpenSession.Session_Open = false;
                    db.Entry(tutorOpenSession).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("TutorLogin");

            }
            return RedirectToAction("StudentQueue", new { tutorSession_id = tutorSession.TutorSession_Id });
        }

        public ActionResult RemoveFromQueue(int studentSession_id, int tutorSession_id)
        {
            var tutorSession = db.TutorSessions.Find(tutorSession_id);
            ViewBag.tutor_id = tutorSession.Tutor_Id;
            var studentSession = db.StudentSessions.Find(studentSession_id);
            ViewBag.StudentFullName = String.Format("{0} {1} {2}", studentSession.Student.Student_FirstName, studentSession.Student.Student_MName, studentSession.Student.Student_LastName);
            ViewBag.CourseDescription = String.Format("{0}-{1} {2}", studentSession.CRNSection.Course_Id, studentSession.CRNSection.CRN_Id, studentSession.CRNSection.Course.Course_Title);

            var sessionTime = new SessionTime();
                sessionTime.StudentSession_Id = studentSession_id;
                sessionTime.TutorSession_Id = tutorSession_id;
        
            return View(sessionTime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromQueue([Bind(Include = "TutorSession_Id,StudentSession_Id")] SessionTime sessionTime,  string BtnPressed)
        {
            var studentSession = db.StudentSessions.Find(sessionTime.StudentSession_Id);
            if (BtnPressed == "Remove")
            {

                studentSession.Session_Active = false;
                if (ModelState.IsValid)
                {
                    db.Entry(studentSession).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("StudentQueue", new { tutorSession_id = sessionTime.TutorSession_Id, filterArea = studentSession.Area_Id });
        }

        private static LCSAP_LC.ViewModels.VMTutorSession VMTS = new VMTutorSession();

        public ActionResult GroupQueueInitial(int tutorsession_id, int? filterArea)
        {

            var tutorSession = db.TutorSessions.Find(tutorsession_id);
            
            if (tutorSession == null)
                return RedirectToAction("TutorLogin");
            ViewBag.AreaFilter = filterArea;
            ViewBag.Tutor_Id = tutorSession.Tutor_Id;
            ViewBag.TutorSession_Id = tutorsession_id;
            ViewBag.Area_Name = "All";
            var studentSession = db.StudentSessions.Where(s => s.Campus_Id == tutorSession.Campus_Id && s.Session_Active).Include(t => t.CRNSection).Include(c => c.CRNSection.Course);
            DateTime ayer = DateTime.Today;
            studentSession = studentSession.Where(d => d.Checkin_Time >= ayer);

            if (filterArea >= 0)
            {
                studentSession = studentSession.Where(x => x.Area_Id == filterArea);
                ViewBag.Area_Name = db.Areas.Find(filterArea).Area_Name;
            }
            studentSession = studentSession.OrderBy(c => c.Checkin_Time);

            VMTS.VMTutorSession_Id = tutorsession_id;
            VMTS.VMTutorDisplayName = tutorSession.Tutor.Tutor_Fname + " " + tutorSession.Tutor.Tutor_Lname;
            VMTS.VMStudentSessions = new List<ViewModels.VMStudentSession>();
            
            foreach (var item in studentSession)
            {
                var VMSSession = new VMStudentSession
                {
                    VMStudentSession_Id = item.StudentSession_Id,
                    VMStudentDisplayName = item.Student.Student_FirstName + " " + item.Student.Student_LastName,
                    VMCourseDisplayName = item.CRNSection.Course_Id + " " + item.CRNSection.Course.Course_Title,
                    VMCheckin_Time = item.Checkin_Time,
                    VMArea_Id = item.Area_Id,
                    VMSelected = false
                };
                VMTS.VMStudentSessions.Add(VMSSession);
            }
                return View("GroupQueue", VMTS);
        }

        public ActionResult GroupQueue(int? studentSession_Id, int? filterArea, bool selectFlag)
        {

            ViewBag.TutorSession_Id = VMTS.VMTutorSession_Id;
            ViewBag.AreaFilter = filterArea;
            ViewBag.TutorSession_Id = VMTS.VMTutorSession_Id;
            ViewBag.Area_Name = "All";
            if (filterArea >= 0)
            {
                ViewBag.Area_Name = db.Areas.Find(filterArea).Area_Name;
            }
            for (int i = 0; i < VMTS.VMStudentSessions.Count; i++)
            {
                if (VMTS.VMStudentSessions[i].VMStudentSession_Id == studentSession_Id)
                    VMTS.VMStudentSessions[i].VMSelected = selectFlag;
            }
            

            return View(VMTS);
        }

        // GET: TutorSessions/CreateTimes
        public ActionResult CreateGroupTimes(int? filterArea)
        {
            ViewBag.AreaFilter = filterArea;
            VMTS.VMTutoring_Start = DateTime.Now;
            foreach (var sSession in VMTS.VMStudentSessions)
            {
                if (sSession.VMSelected)
                {
                    var studentSession = db.StudentSessions.Find(sSession.VMStudentSession_Id);
                    TempRemove(studentSession);
                }

            }
            
            return View(VMTS);
        }


        // POST: TutorSessions/CreateTimes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateGroupTimes()
        {

            var s_active = Request.Form["session_active"];
            var filterArea = Request["AreaFilter"];
            bool session_active = s_active == "on" ? true : false;

            foreach (var sSession in VMTS.VMStudentSessions)
            {
                if (sSession.VMSelected)
                {
                    var sessionTime = new SessionTime();
                    var studentSession = db.StudentSessions.Find(sSession.VMStudentSession_Id);
                    sessionTime.StudentSession_Id = sSession.VMStudentSession_Id;
                    sessionTime.TutorSession_Id = VMTS.VMTutorSession_Id;
                    sessionTime.Tutoring_End = DateTime.Now;
                    studentSession.Session_Active = session_active;
                    sessionTime.WaitTime_Start = studentSession.Checkin_Time;
                    sessionTime.Tutoring_Start = VMTS.VMTutoring_Start;
                    studentSession.Checkin_Time = DateTime.Now;
                    if (ModelState.IsValid)
                    {
                        db.SessionTimes.Add(sessionTime);

                        db.Entry(studentSession).State = EntityState.Modified;
                    }
                }
            }
            await db.SaveChangesAsync();
            return RedirectToAction("GroupQueueInitial", new { tutorSession_id = VMTS.VMTutorSession_Id, filterArea });
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