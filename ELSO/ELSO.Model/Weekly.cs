namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WK_REF")]
    public partial class Weekly : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Weekly()
        {
            WeeklyReccurences = new HashSet<WeeklyReccurence>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None),Column("WK_UID")]
        public int Id { get; set; }

        [Required]
        [StringLength(254)]
        [Column("WK_DESC")]
        public string WeekDescp { get; set; }

        [Column("MNTH_SCP_SW")]
        public bool MScopeSwitch { get; set; }

        [Column("ANNL_SCP_SW")]
        public bool YScopeSwitch { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeeklyReccurence> WeeklyReccurences { get; set; }
    }
}
