using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LCSAP_LC.Models;
using System.Threading.Tasks;
using LCSAP_LC.Services;

namespace LCSAP_LC.Controllers
{
    public class StudentSessionsController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();
        private LCSAPServices lcServices = new LCSAPServices();


        // GET: StudentSessions
        public ActionResult Index()
        {
            
            return RedirectToAction("SelectCampus");
        }

        // GET: StudentSessions/SelectCampus
        public ActionResult SelectCampus()
        {
            var campuses = db.Campuses;
            return View(campuses);
        }

        // GET: StudentSessions/SelectArea
        public ActionResult SelectArea(string campus_id)
        {
            ViewBag.Campus_Id = campus_id;
            ViewBag.Campus_Name = db.Campuses.Find(campus_id).Campus_Name;
            var areas = db.Areas;
            return View(areas);
        }
        // GET: StudentSessions/StudentLogin
        public ActionResult StudentLogin(string campus_id, int area_id, int? errorMsg)
        {
            ViewBag.errorMessage = (errorMsg == 1 ? "Id Not Found, Try Again" : errorMsg == 2 ? "No Courses Found!" : errorMsg == 3 ? "You are already logged In!" : errorMsg == 4 ? "No Tutor Session Available - Try again Later" : "");
            ViewBag.Campus_Id = campus_id;
            ViewBag.CampusName = db.Campuses.Find(campus_id).Campus_Name;
            ViewBag.AreaName = db.Areas.Find(area_id).Area_Name;
            ViewBag.Area_Id = area_id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentLogin([Bind(Include = "Student_Id,Area_Id,Campus_Id")] StudentSession studentSession)
        {
            int errMsg = 0;
            var findOpenTutorSession = db.TutorSessions.Where(s => s.Session_Open && s.Campus_Id == studentSession.Campus_Id).FirstOrDefault();
            if (findOpenTutorSession != null)
            {
                try
                {
                    var findOpenSession = db.StudentSessions.Where(s => s.Student_Id == studentSession.Student_Id && s.Session_Active).FirstOrDefault();
                    if (findOpenSession == null)
                    {
                        var findStudent = db.Students.Find(studentSession.Student_Id);
                        if (findStudent != null)
                        {
                            return RedirectToAction("OpenStudentSession", new { campus_id = studentSession.Campus_Id, area_id = studentSession.Area_Id, student_id = studentSession.Student_Id });
                        }
                        else { errMsg = 1; }
                    }
                    else { errMsg = 3; }
                }
                catch
                {

                }
            }
            else { errMsg = 4; }
            return RedirectToAction("StudentLogin", new { campus_id = studentSession.Campus_Id, area_id = studentSession.Area_Id, errorMsg = errMsg });
        }

        // GET: StudentSessions/OpenStudentSession
        public ActionResult OpenStudentSession(string campus_id, int area_id, string student_id)
        {
            var studentSession = new StudentSession();
            var student = db.Students.Include(a => a.CRNSections).Where(a => a.Student_Id == student_id);

            //.Where(b => b.Term.Term_End >= DateTime.Now)
            if (student == null)
                RedirectToAction("StudentLogin", new { campus_id, area_id, errorMsg = 1 });
            var studentSections = student.SelectMany(s => s.CRNSections).Where(b => b.Term.Term_Id == lcServices.CurrentTerm);
            ViewBag.CRN_Id = new SelectList(studentSections, "CRN_Id", "Course_Id");
            if (studentSections.Count() == 0)
                RedirectToAction("StudentLogin", new { campus_id, area_id, errorMsg = 2 });
            studentSession.Student_Id = student_id;
            studentSession.Campus_Id = campus_id;
            studentSession.Area_Id = area_id;
            return View(studentSession);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> OpenStudentSession([Bind(Include = "StudentSession_Id,Checkin_Time,Session_Active,Student_Id,CRN_Id,Area_Id,Campus_Id")] StudentSession studentSession)
        {
            studentSession.Checkin_Time = DateTime.Now;
            studentSession.Session_Active = true;
            if (ModelState.IsValid)
            {
                db.StudentSessions.Add(studentSession);
                await db.SaveChangesAsync();
                
            }
            return RedirectToAction("StudentLogin", new { campus_id = studentSession.Campus_Id, area_id = studentSession.Area_Id, errorMsg = 0 });
        }

        public ActionResult SwitchArea(int tutorSession_Id, int studentSession_Id)
        {
            var studentSession = db.StudentSessions.Find(studentSession_Id);
            ViewBag.StudentSession_Id = studentSession.StudentSession_Id;
            ViewBag.TutorSession_Id = tutorSession_Id;
            var areas = db.Areas;
            return View(areas);
        }
        
        public ActionResult UpdateArea(int tutorSession_Id, int studentSession_Id, int area_id)
        {
            var studentSession = db.StudentSessions.Find(studentSession_Id);
            studentSession.Area_Id = area_id;
            db.Entry(studentSession).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("StudentQueue", "TutorSessions", new { tutorSession_Id, filterArea = area_id}) ;
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
