namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MessageNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MessageNote()
        {
            Communications = new HashSet<Communication>();
        }

        [Key]
        public int Message_Id { get; set; }

        public DateTime? Message_Date { get; set; }

        [StringLength(300)]
        public string Message_Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Communication> Communications { get; set; }
    }
}
