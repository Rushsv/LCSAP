namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TutorSession
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TutorSession()
        {
            SessionTimes = new HashSet<SessionTime>();
        }

        [Key]
        public int TutorSession_Id { get; set; }

        public DateTime Session_Date { get; set; }

        public bool Session_Open { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Session Cost")]
        [DataType(DataType.Currency)]
        public decimal Session_Cost { get; set; }

        [Required]
        [StringLength(9)]
        [RegularExpression("^A[0-9]+$", ErrorMessage = "Invalid ID Format: A########")]
        public string Tutor_Id { get; set; }

        public int Style_Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Campus_Id { get; set; }

        public virtual Campus Campus { get; set; }

        public virtual SessionStyle SessionStyle { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SessionTime> SessionTimes { get; set; }

        public virtual Tutor Tutor { get; set; }
    }
}
