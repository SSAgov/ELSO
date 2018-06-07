namespace ELSO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EVNT")]
    public partial class Event : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Event()
        {
            EventMaxCount = new HashSet<EventMaxCount>();
            Registrations = new HashSet<Registration>();
            Patterns = new HashSet<Pattern>();
            Sessions = new HashSet<Session>();
        }

        [Key]
        [Column("EVNT_UID")]
        public int Id { get; set; }

        [Column("CNCLN_DTM", TypeName = "datetime2")]
        public DateTime? CancellationDate { get; set; }

        [Required]
        [StringLength(62)]
        [Column("EVNT_NM")]
        public string EventName { get; set; }

        [StringLength(4)]
        [Column("EVNT_TYP_CD")]
        public string ETypeCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventMaxCount> EventMaxCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Registration> Registrations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pattern> Patterns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Session> Sessions { get; set; }

        public virtual EventType EventType { get; set; }
    }
}
