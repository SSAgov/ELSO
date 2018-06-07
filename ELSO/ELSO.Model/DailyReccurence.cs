namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ANNL_AND_MNTHL_AND_WKLRCUR_PATRN_DAYS")]
    public partial class DailyReccurence : BaseEntity
    {
        [Key]
        [Column("DAY_UID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column("RCURPATRN_UID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecurrenceId { get; set; }

        public virtual Daily Daily { get; set; }

        public virtual Pattern Pattern { get; set; }
    }
}
