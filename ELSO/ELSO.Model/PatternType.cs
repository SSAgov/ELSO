namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RCURPATRN_TYP_REF")]
    public partial class PatternType : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PatternType()
        {
            Patterns = new HashSet<Pattern>();
        }

        [Key]
        [Column("RCURPATRN_TYP_UID")]
        public int Id { get; set; }

        [Required]
        [StringLength(254)]
        [Column("RCURPATRN_TPDESC")]
        public string PatternDescp { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pattern> Patterns { get; set; }
    }
}
