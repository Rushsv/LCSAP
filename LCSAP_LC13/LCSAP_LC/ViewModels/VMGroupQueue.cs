using LCSAP_LC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCSAP_LC.ViewModels
{
    public class VMTutorSession
    {
        public int VMTutorSession_Id { get; set; }
        public string VMTutorDisplayName { get; set; }
        public DateTime VMTutoring_Start { get; set; }
        public List<VMStudentSession> VMStudentSessions { get; set; }
    }

    public class VMStudentSession
    {
        public int VMStudentSession_Id { get; set; }
        public string VMStudentDisplayName { get; set; }
        public string VMCourseDisplayName { get; set; }
        public DateTime VMCheckin_Time { get; set; }
        public int VMArea_Id { get; set; }
        public bool VMSelected { get; set; }
    }
}