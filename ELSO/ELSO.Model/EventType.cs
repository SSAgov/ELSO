namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EVNT_TYP_CD_REF")]
    public partial class EventType : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EventType()
        {
            Events = new HashSet<Event>();
        }

        [Key]
        [StringLength(4)]
        [Column("EVNT_TYP_CD")]
        public string ETypeCode { get; set; }

        [Required]
        [StringLength(254)]
        [Column("EVNT_TYP_CDESC")]
        public string EventDescp { get; set; }

        [Column("EFF_STDTM", TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column("EFF_ENDTM", TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Events { get; set; }
    }
}
