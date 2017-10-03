namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StudentSession
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentSession()
        {
            SessionTimes = new HashSet<SessionTime>();
        }

        [Key]
        public int StudentSession_Id { get; set; }

        public DateTime Checkin_Time { get; set; }

        public bool Session_Active { get; set; }

        [Required]
        [StringLength(9)]
        [RegularExpression("^A[0-9]+$", ErrorMessage = "Invalid ID Format: A########")]
        public string Student_Id { get; set; }

        public int CRN_Id { get; set; }

        public int Area_Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Campus_Id { get; set; }

        public virtual Area Area { get; set; }

        public virtual Campus Campus { get; set; }

        public virtual CRNSection CRNSection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SessionTime> SessionTimes { get; set; }

        public virtual Student Student { get; set; }
    }
}
