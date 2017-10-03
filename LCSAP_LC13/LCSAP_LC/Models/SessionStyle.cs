namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SessionStyle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SessionStyle()
        {
            TutorSessions = new HashSet<TutorSession>();
        }

        [Key]
        public int Style_Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Session Style")]
        public string Style_Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TutorSession> TutorSessions { get; set; }
    }
}
