namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Courses")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            CRNSections = new HashSet<CRNSection>();
        }

        [Key]
        [StringLength(8)]
        public string Course_Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Course Title")]
        public string Course_Title { get; set; }

        [Required]
        [StringLength(4)]
        public string Subject_Id { get; set; }

        public virtual Subject Subject { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRNSection> CRNSections { get; set; }
    }
}
