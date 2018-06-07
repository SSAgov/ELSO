namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RCURPATRN")]
    public partial class Pattern : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pattern()
        {
            DailyReccurences = new HashSet<DailyReccurence>();
            WeeklyReccurences = new HashSet<WeeklyReccurence>();
            MonthlyReccurences = new HashSet<MonthlyReccurence>();
            Sessions = new HashSet<Session>();
        }

        [Key]
        [Column("RCURPATRN_UID")]
        public int Id { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }
        [Column("RCURPSINTVL_CNT")]
        public int PatternStartCount { get; set; }
        [Column("RCURPATRN_CNT")]
        public int PatternCount { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [Column("CNCLD_DTM", TypeName = "datetime2")]
        public DateTime? CancellationDate { get; set; }

        [Column("RCURPATRN_TYP_UID")]
        public int PatternTypeId { get; set; }

        [Column("EVNT_UID")]
        public int EventId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyReccurence> DailyReccurences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeeklyReccurence> WeeklyReccurences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonthlyReccurence> MonthlyReccurences { get; set; }

        public virtual Event Event { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Session> Sessions { get; set; }

        public virtual PatternType PatternType { get; set; }
    }
}
