using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ELSO.Model
{
    public class EventCategory : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EventCategory()
        {
            Meetings = new HashSet<Event>();
        }

        [Key]
        public int MTG_CTGY_UID { get; set; }

        [Required]
        [StringLength(254)]
        public string MTG_CTGY_DESC { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LU_TS { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime INSRT_TS { get; set; }


        // NAVIGATION
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Event> Meetings { get; set; }

    }
}
