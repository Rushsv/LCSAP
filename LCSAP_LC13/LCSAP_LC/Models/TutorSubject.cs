namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TutorSubject
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(9)]
        public string Tutor_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string Subject_Id { get; set; }
    }
}
