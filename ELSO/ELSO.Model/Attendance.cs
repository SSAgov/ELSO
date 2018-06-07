namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRSRPRTCPN")]
    public partial class Attendance : BaseEntity
    {
        [Key]
        [Column("PRSRPRTCPN_UID")]
        public int Id { get; set; }

        [Column("RCURPATRN_SESN_UID")]
        public int SessionId { get; set; }

        [Column("PRTCPG_PRSN_ID")]
        public int PersonId { get; set; }

        [Required]
        [StringLength(4)]
        [Column("PRSN_ROLE_CD")]
        public string RoleCode { get; set; }

        [Column("PRTCPD_DTM", TypeName = "datetime2")]
        public DateTime ParticipatedDate { get; set; }

        public virtual Person Person { get; set; }

        public virtual Role Role { get; set; }

        public virtual Session Session { get; set; }
    }
}
