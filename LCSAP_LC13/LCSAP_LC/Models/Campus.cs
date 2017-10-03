namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Campuses")]
    public partial class Campus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Campus()
        {
            StudentSessions = new HashSet<StudentSession>();
            TutorSessions = new HashSet<TutorSession>();
        }

        [Key]
        [StringLength(2)]
        public string Campus_Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Campus")]
        public string Campus_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentSession> StudentSessions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TutorSession> TutorSessions { get; set; }
    }
}
