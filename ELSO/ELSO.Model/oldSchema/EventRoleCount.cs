namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EVNT_PRSN_ROLE_MAX_CNT")]
    public partial class EventRoleCount : BaseEntity
    {
        [Key]
        [Column("PRSN_ROLE_CD",Order = 0)]
        [StringLength(4)]
        public string RoleCode { get; set; }

        [Key]
        [Column("EVNT_UID", Order = 1)]
        public int EventId { get; set; }

        public int MAX_PRSN_CNT { get; set; }

        public virtual Event Event { get; set; }

        public virtual Role Role { get; set; }
    }
}
