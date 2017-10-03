using System;
using System.Linq;
using System.Data.SqlClient;
using System.Web.Mvc;
using LCSAP_LC.Models;
using LCSAP_LC.Services;
using LCSAP_LC.ViewModels;

namespace LCSAP_LC.Controllers
{

    public class SessionReportsController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();
        private LCSAPServices lcServices = new LCSAPServices();

        // GET: SessionReports
        public ActionResult CostPerStudent(string termFilter, string campusFilter, int? areaFilter, string studentFilter)

        {
            if (termFilter == null || termFilter == string.Empty)
                termFilter = lcServices.CurrentTerm;
            StudentSessionsDetail studentDetails = new StudentSessionsDetail();
            var studentSessions = db.StudentSessions.Where(a => a.CRNSection.Term_Id == termFilter.Trim()).OrderBy(a => a.Student_Id);
            var allSessions =
                from a in db.Students
                join b in studentSessions on a.Student_Id equals b.Student_Id
                join c in db.SessionTimes on b.StudentSession_Id equals c.StudentSession_Id
                join t in db.TutorSessions on c.TutorSession_Id equals t.TutorSession_Id
                orderby a.Student_Id
                select new StudentSessionsDetail()
                {
                    StudentId = a.Student_Id,
                    FullName = a.Student_FirstName + " " + a.Student_LastName,
                    AreaId = b.Area_Id,
                    CampusId = b.Campus_Id,
                    CampusName = b.Campus.Campus_Name,
                    AreaName = b.Area.Area_Name,
                    TutorId = t.Tutor_Id,
                    TutorName = t.Tutor.Tutor_Fname,
                    HrCost = t.Session_Cost,
                    WTime = c.WaitTime_Start,
                    TTimeStart = c.Tutoring_Start,
                    TTimeEnd = c.Tutoring_End
                    
                };

            if (!String.IsNullOrEmpty(studentFilter))
            {
                allSessions = allSessions.Where(x => x.FullName.Contains(studentFilter)
                                       || x.StudentId.Contains(studentFilter));
            }

            if (campusFilter != null && campusFilter != string.Empty)
                allSessions = allSessions.Where(x => x.CampusId == campusFilter);

            if (areaFilter != null && areaFilter != 0)
                allSessions = allSessions.Where(x => x.AreaId == areaFilter);

            ViewBag.termFilter = new SelectList(db.Terms, "Term_Id", "Term_Name");
            ViewBag.campusFilter = new SelectList(db.Campuses, "Campus_Id", "Campus_Name");
            ViewBag.areaFilter = new SelectList(db.Areas, "Area_Id", "Area_Name");
            ViewBag.studentFilter = studentFilter;

            return View(allSessions);
        }

        public ActionResult SessionsPerStudent(string termFilter, string campusFilter, string studentFilter)

        {
            if (termFilter == null || termFilter == string.Empty)
                termFilter = lcServices.CurrentTerm;
            StudentSessionsSummary studentSummary = new StudentSessionsSummary();
            var studentSessions = db.StudentSessions.Where(a => a.CRNSection.Term_Id == termFilter.Trim()).OrderBy(a => a.Student_Id);
            var allSessions =
                from a in db.Students
                join b in studentSessions on a.Student_Id equals b.Student_Id
                join c in db.SessionTimes on b.StudentSession_Id equals c.StudentSession_Id
                join t in db.TutorSessions on c.TutorSession_Id equals t.TutorSession_Id
                orderby a.Student_Id
                select new StudentSessionsDetail() 
                {
                    StudentId = a.Student_Id,
                    FullName = a.Student_FirstName + " " + a.Student_LastName,
                    AreaId = b.Area_Id,
                    CampusId = b.Campus_Id,
                    CampusName = b.Campus.Campus_Name,
                    AreaName = b.Area.Area_Name,
                    TutorId = t.Tutor_Id,
                    TutorName = t.Tutor.Tutor_Fname,
                    HrCost = t.Session_Cost,
                    WTime = c.WaitTime_Start,
                    TTimeStart = c.Tutoring_Start,
                    TTimeEnd = c.Tutoring_End
                    
                };
            //Grouped By Student
            var studentsGrouped =
                from a in allSessions
                orderby a.StudentId
                group a by a.StudentId into b
                from c in b
                let sessionsCount = b.Count()
                let sessionsAvgCost = b.Average(t => t.HrCost)
                orderby c.StudentId
                select new StudentSessionsSummary()
                {
                    StudentId = c.StudentId,
                    FullName = c.FullName,
                    CampusId = c.CampusId,
                    CampusName = c.CampusName,
                    HrCost = sessionsAvgCost,
                    SecondsTotal = 0,
                    SessionsTotal = sessionsCount
                };
            studentsGrouped = studentsGrouped.Distinct();
            
            if (!String.IsNullOrEmpty(studentFilter))
            {
                studentsGrouped = studentsGrouped.Where(x => x.FullName.Contains(studentFilter)
                                       || x.StudentId.Contains(studentFilter));
            }

            if (campusFilter != null && campusFilter != string.Empty)
                studentsGrouped = studentsGrouped.Where(x => x.CampusId == campusFilter);

            
            ViewBag.termFilter = new SelectList(db.Terms, "Term_Id", "Term_Name");
            ViewBag.campusFilter = new SelectList(db.Campuses, "Campus_Id", "Campus_Name");
            ViewBag.studentFilter = studentFilter;

            return View(studentsGrouped);
        }

        public ActionResult CostPerTutor(string termFilter, string campusFilter, int? areaFilter, string tutorFilter)

        {
            if (termFilter == null || termFilter == string.Empty)
                termFilter = lcServices.CurrentTerm;
            var studentSessions = db.StudentSessions.Where(a => a.CRNSection.Term_Id == termFilter.Trim()).OrderBy(a => a.Student_Id);
            var allSessions =
                from a in db.Students
                join b in studentSessions on a.Student_Id equals b.Student_Id
                join c in db.SessionTimes on b.StudentSession_Id equals c.StudentSession_Id
                join t in db.TutorSessions on c.TutorSession_Id equals t.TutorSession_Id
                orderby t.Tutor_Id
                select new StudentSessionsDetail()
                {
                    StudentId = a.Student_Id,
                    FullName = a.Student_FirstName + " " + a.Student_LastName,
                    AreaId = b.Area_Id,
                    CampusId = b.Campus_Id,
                    CampusName = b.Campus.Campus_Name,
                    AreaName = b.Area.Area_Name,
                    TutorId = t.Tutor_Id,
                    TutorName = t.Tutor.Tutor_Fname+" "+t.Tutor.Tutor_Lname,
                    HrCost = t.Session_Cost,
                    WTime = c.WaitTime_Start,
                    TTimeStart = c.Tutoring_Start,
                    TTimeEnd = c.Tutoring_End
                };

            if (!String.IsNullOrEmpty(tutorFilter))
            {
                allSessions = allSessions.Where(x => x.TutorName.Contains(tutorFilter)
                                       || x.TutorId.Contains(tutorFilter));
            }

            if (campusFilter != null && campusFilter != string.Empty)
                allSessions = allSessions.Where(x => x.CampusId == campusFilter);

            if (areaFilter != null && areaFilter != 0)
                allSessions = allSessions.Where(x => x.AreaId == areaFilter);

            ViewBag.termFilter = new SelectList(db.Terms, "Term_Id", "Term_Name");
            ViewBag.campusFilter = new SelectList(db.Campuses, "Campus_Id", "Campus_Name");
            ViewBag.areaFilter = new SelectList(db.Areas, "Area_Id", "Area_Name");
            ViewBag.studentFilter = tutorFilter;

            return View(allSessions);
        }


        private double SecondsCalc(DateTime tutoring_End, DateTime tutoring_Start)
        {
            return tutoring_End.Subtract(tutoring_Start).TotalSeconds;
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