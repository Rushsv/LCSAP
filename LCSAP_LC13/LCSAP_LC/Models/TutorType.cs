namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TutorType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TutorType()
        {
            Tutors = new HashSet<Tutor>();
        }

        [Key]
        public int TutorType_Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Tutor Type")]
        public string TutorType_description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tutor> Tutors { get; set; }
    }
}
