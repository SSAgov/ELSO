namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRERASGNT")]
    public partial class Registration : BaseEntity
    {
        [Key]
        [Column("PRERASGNT_UID")]
        public int Id { get; set; }

        [Column("ASGNE_PRSN_ID")]
        public int PersonId { get; set; }

        [Column("EVNT_UID")]
        public int? EventId { get; set; }

        [StringLength(4),Column("PRSN_ROLE_CD")]
        public string RoleCode { get; set; }

        [Column("ASGND_DTM", TypeName = "datetime2")]
        public DateTime AssignmentDate { get; set; }

        public virtual Event Event { get; set; }

        public virtual Person Person { get; set; }

        public virtual Role Role { get; set; }
    }
}
