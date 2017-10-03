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
using LCSAP_LC.Services;

namespace LCSAP_LC.Controllers
{
    public class StudentsController : Controller
    {
        private LCSAPDbContext db = new LCSAPDbContext();
        private LCSAPServices lcServices = new LCSAPServices();


        // GET: Students
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

            var students = from s in db.Students
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Student_LastName.Contains(searchString)
                                       || s.Student_MName.Contains(searchString)
                                       || s.Student_FirstName.Contains(searchString)
                                       || s.Student_Id.Contains(searchString));
            }


            students = students.OrderBy(s => s.Student_LastName);

            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
            
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = id.ToUpper();
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }


            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Student_Id,Student_FirstName,Student_MName,Student_LastName,Student_DOB,Student_Gender,Student_Major")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Student_Id,Student_FirstName,Student_MName,Student_LastName,Student_DOB,Student_Gender,Student_Major")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        public ActionResult Enroll(string id)
        {
            var cRNSections = db.CRNSections.Include(c => c.Course).Include(c => c.Term);
            cRNSections = cRNSections.Where(b => b.Term.Term_Id == lcServices.CurrentTerm)
                .OrderBy(b => b.Course.Course_Title);
            var ListcRNSections = cRNSections.Select(b => new {CRN_Id = b.CRN_Id,Description = b.CRN_Id+ " - "+ b.Course.Course_Title})
                .ToList();
            //ViewBag.Course_Id = new SelectList(db.Courses, "Course_Id", "Course_Title");
            ViewBag.CRN_Id = new SelectList(ListcRNSections, "CRN_Id", "Description");
            ViewBag.Student_Id = id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroll([Bind(Include = "Student_Id")] Student studentid, int crn_Id)
        {
            var crnSection = db.CRNSections.Find(crn_Id);
            var student = db.Students.Find(studentid.Student_Id);
            //if (ModelState.IsValid)
            try
            {
                db.CRNSections.Attach(crnSection);
                db.Students.Attach(student);
                //db.Entry(student).State = EntityState.Modified;
                student.CRNSections.Add(crnSection);
                db.SaveChanges();
                
            }
            catch { }
            return RedirectToAction("Details", new { id = student.Student_Id });
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Unenroll(string student_id, int crn_id)
        {
            if (student_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRNSection crnSection = db.CRNSections.Find(crn_id);
            Student student = db.Students.Find(student_id);
            ViewBag.CRN_Id = crn_id;
            ViewBag.Course_Id = crnSection.Course_Id;
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Unenroll")]
        [ValidateAntiForgeryToken]
        public ActionResult UnenrollConfirmed(string student_id, int crn_id)
        {
            Student student = db.Students.Find(student_id);
            CRNSection crnSection = db.CRNSections.Find(crn_id);
            try { 
            student.CRNSections.Remove(crnSection);
            db.SaveChanges();
            }
            catch { }
            return RedirectToAction("Details",new {id = student_id });
        }

        public ActionResult StudentVerify( int? errorMsg)
        {
            ViewBag.errorMessage = (errorMsg == 1 ? "Id Not Found, Try Again" : "");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentVerify(Student student)
        {
            
            student = db.Students.Find(student.Student_Id);
            
            if (student == null) 
                return RedirectToAction("StudentVerify", new { errorMsg = 1 });
            
            int enrolled = student.CRNSections.Where(s => s.Term.Term_Id == lcServices.CurrentTerm).Count();
            var FullName = student.Student_FirstName + " " + student.Student_LastName;
            ViewBag.GeneralMessage = String.Format("{0} Courses Found for student {1}",enrolled,FullName);
            ViewBag.ErrorMsg = 0;
            return View();
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
