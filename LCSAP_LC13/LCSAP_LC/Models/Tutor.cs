namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tutor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tutor()
        {
            Communications = new HashSet<Communication>();
            TutorSessions = new HashSet<TutorSession>();
            Subjects = new HashSet<Subject>();
        }

        [Key]
        [Required]
        [RegularExpression("^A[0-9]+$", ErrorMessage = "Invalid ID Format: A#")]
        [StringLength(9)]
        [Display(Name = "Staff AIMS-ID", Prompt = "A########")]
        public string Tutor_Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string Tutor_Fname { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string Tutor_Lname { get; set; }

        [Column(TypeName = "money")]
        [Display(Name = "Pay Rate")]
        [DataType(DataType.Currency)]
        public decimal Tutor_PayRate { get; set; }

        [Display(Name = "Active")]
        public bool Tutor_Active { get; set; }

        public int TutorType_Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Communication> Communications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TutorSession> TutorSessions { get; set; }

        public virtual TutorType TutorType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
