namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PRSN_ROLE_ASGNT")]
    public partial class PersonRole : BaseEntity
    {
        [Key]
        [Column("PRSN_ROLE_ASGNT_UID")]
        public int Id { get; set; }

        [Column("ASGNE_PRSN_ID")]
        public int PersonId { get; set; }

        [StringLength(4)]
        [Column("PRSN_ROLE_CD")]
        public string RoleCode { get; set; }

        [Column("ASGND_DTM", TypeName = "datetime2")]
        public DateTime AssignedDate { get; set; }

        public virtual Person Person { get; set; }

        public virtual Role Role { get; set; }
    }
}
