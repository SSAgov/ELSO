namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RCURPATRN_SESN")]
    public partial class Session : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Session()
        {
            Attendances = new HashSet<Attendance>();
        }

        [Key]
        [Column("RCURPATRN_SESN_UID")]
        public int SessionId { get; set; }

        [Column("CNCLN_DTM", TypeName = "datetime2")]
        public DateTime? CancellationDate { get; set; }

        [Column("ORIGL_STDTM", TypeName = "datetime2")]
        public DateTime OriginalStartDate { get; set; }

        [Column("RSCHD_DTM", TypeName = "datetime2")]
        public DateTime? RescheduledDate { get; set; }

        [Column("RCURPATRN_UID")]
        public int PatternId { get; set; }

        [Column("RCURPATRN_CYCL_ID")]
        public int PatternCycleId { get; set; }

        [Column("EVNT_UID")]
        public int EventId { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [StringLength(254), Column("LOC_TXT")]
        public string Location { get; set; }

        public virtual Event Event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }

        public virtual Pattern Pattern { get; set; }
    }
}
