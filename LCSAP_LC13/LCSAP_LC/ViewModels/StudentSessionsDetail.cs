using LCSAP_LC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LCSAP_LC.ViewModels
{
    public class StudentSessionsDetail
    {

        public string StudentId { get; set; }
        public string FullName { get; set; }
        public string CampusId { get; set; }
        public string CampusName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string TutorId { get; set; }
        public string TutorName { get; set; }
        [DataType(DataType.Currency)]
        public decimal HrCost { get; set; }
        public DateTime WTime { get; set; }
        public DateTime TTimeStart { get; set; }
        public DateTime TTimeEnd { get; set; }
        


        

    }
}