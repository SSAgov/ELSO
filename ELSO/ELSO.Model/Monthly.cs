namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MNTH_REF")]
    public partial class Monthly : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Monthly()
        {
            MonthlyReccurence = new HashSet<MonthlyReccurence>();
        }

        [Key]
        [Column("MNTH_UID")]
        public int Id { get; set; }

        [Required]
        [StringLength(254)]
        [Column("MNTH_DESC")]
        public string MonthDescp { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonthlyReccurence> MonthlyReccurence { get; set; }
    }
}
