namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Subject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Subject()
        {
            Courses = new HashSet<Course>();
            Tutors = new HashSet<Tutor>();
        }

        [Key]
        [StringLength(4)]
        public string Subject_Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Subject")]
        public string Subject_Name { get; set; }

        [Display(Name = "Not From Catalog?")]
        public bool? Not_Catalog { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tutor> Tutors { get; set; }
    }
}
