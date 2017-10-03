namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            StudentSessions = new HashSet<StudentSession>();
            CRNSections = new HashSet<CRNSection>();
        }

        [Key]
        [StringLength(9)]
        [RegularExpression("^A[0-9]+$", ErrorMessage ="Invalid ID Format: A########")]
        [Display(Name = "AIMS ID", Prompt ="A########")]
        [Required]
        public string Student_Id { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "First Name")]
        public string Student_FirstName { get; set; }

        [StringLength(25)]
        [Display(Name = "Middle Name")]
        public string Student_MName { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Last Name")]
        public string Student_LastName { get; set; }

        [Display(Name = "Birth Year")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy}")]
        public DateTime? Student_DOB { get; set; }

        [StringLength(1)]
        [Display(Name = "Gender")]
        public string Student_Gender { get; set; }

        [StringLength(100)]
        [Display(Name = "Major")]
        public string Student_Major { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentSession> StudentSessions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRNSection> CRNSections { get; set; }
    }
}
