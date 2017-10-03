using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LCSAP_LC.Models;
using System.Data.Entity;

namespace LCSAP_LC.Services
{

    public class LCSAPServices
    {
        public string CurrentTerm { get; set; }
        private LCSAPDbContext db = new LCSAPDbContext();

        public LCSAPServices()
        {
            CurrentTerm = FindCurrentTerm();
        }
        private String FindCurrentTerm()
        {
            Term term = db.Terms.Where(t => t.Term_Start <= DateTime.Now && t.Term_End >= DateTime.Now).FirstOrDefault();
            var currentTerm = (term == null ? "" : term.Term_Id);
            return currentTerm;
        }
        public bool SeekTerm(string searchTerm)
        {
            bool foundRecord = false;
            Term termFound = db.Terms.Find(searchTerm);
            foundRecord = (termFound == null ? false : true);
            return foundRecord;
        }

        /*public IList<Student> TermStudents (string iTerm )
        {
            var students = db.Students.Include(s=>s.CRNSections);
            
            return students;
        }
        */
        public void UpdateActiveQueue()
        {
            DateTime ayer = DateTime.Today;
            
            //Remove old open student sessions
            var studentSessions = db.StudentSessions.Where(d => d.Checkin_Time <= ayer);
            
            bool oldActives = studentSessions.Count() > 0 ? true :false;
            foreach (var studentSession in studentSessions)
            {
                studentSession.Session_Active = false;
                db.Entry(studentSession).State = EntityState.Modified;
                
            }

            //Remove old open Tutor Sessions.
            var tutorSessions = db.TutorSessions.Where(d => d.Session_Date <= ayer);
            oldActives = tutorSessions.Count() > 0 ? true : false;
            foreach (var tutorSession in tutorSessions)
            {
                tutorSession.Session_Open = false;
                db.Entry(tutorSession).State = EntityState.Modified;
                oldActives = true;
            }
            if (oldActives)
                db.SaveChanges();
            
        }
    }
}