namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Term
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Term()
        {
            CRNSections = new HashSet<CRNSection>();
        }

        [Key]
        [Required]
        [StringLength(10)]
        [RegularExpression(@"^[A-Z0-9]{1,40}$",
         ErrorMessage = "Characters are not allowed. Use Uppercase")]
        public string Term_Id { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Term")]
        public string Term_Name { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime Term_Start { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [Required]
        
        public DateTime Term_End { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CRNSection> CRNSections { get; set; }
    }
}
