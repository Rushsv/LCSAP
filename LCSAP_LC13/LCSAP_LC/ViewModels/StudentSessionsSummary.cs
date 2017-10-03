using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LCSAP_LC.ViewModels
{
    public class StudentSessionsSummary
    {
            public string StudentId { get; set; }
            public string FullName { get; set; }
            public string CampusId { get; set; }
            public string CampusName { get; set; }
            [DataType(DataType.Currency)]
            public decimal HrCost { get; set; }
            public Int32 SecondsTotal { get; set; }
            public Int32 SessionsTotal { get; set; }
    }
}