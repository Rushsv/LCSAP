namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SessionTime
    {
        [Key]
        public int SessionTime_Id { get; set; }

        public int TutorSession_Id { get; set; }

        public int StudentSession_Id { get; set; }

        public DateTime WaitTime_Start { get; set; }

        public DateTime Tutoring_Start { get; set; }

        public DateTime Tutoring_End { get; set; }

        public virtual StudentSession StudentSession { get; set; }

        public virtual TutorSession TutorSession { get; set; }
    }
}
