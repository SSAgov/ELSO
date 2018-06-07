namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DAY_REF")]
    public partial class Daily : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Daily()
        {
            DailyReccurence = new HashSet<DailyReccurence>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None),Column("DAY_UID")]
        public int Id { get; set; }

        [Required]
        [StringLength(254)]
        [Column("DAY_DESC")]
        public string DayDescp { get; set; }

        [Column("WK_SCP_SW")]
        public bool WScopeSwitch { get; set; }

        [Column("MNTH_SCP_SW")]
        public bool MScopeSwitch { get; set; }

        [Column("ANNL_SCP_SW")]
        public bool YScopeSwitch { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyReccurence> DailyReccurence { get; set; }
    }
}
