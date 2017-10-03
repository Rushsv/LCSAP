namespace LCSAP_LC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Communication
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Message_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(9)]
        [RegularExpression("^A[0-9]+$", ErrorMessage = "Invalid ID Format: A########")]
        public string Tutor_Id { get; set; }

        public bool? Message_Read { get; set; }

        public virtual MessageNote MessageNote { get; set; }

        public virtual Tutor Tutor { get; set; }
    }
}
