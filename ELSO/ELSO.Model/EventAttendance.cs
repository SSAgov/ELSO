namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class EventAttendance: BaseEntity 
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PRSN_UID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MTG_UID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime INSRT_TS { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LU_TS { get; set; }

        // NAVIGATION
        public virtual Event MTG { get; set; }
        public virtual Person MTG_PRSN { get; set; }
    }
}
