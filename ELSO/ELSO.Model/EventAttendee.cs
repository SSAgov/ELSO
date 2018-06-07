using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELSO.Model
{
    public class EventAttendee : BaseEntity
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public int PersonId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime AttendedDate { get; set; }
        public bool IsSignedIn { get; set; }
        public bool IsOrganizer { get; set; }

        //  NAVIGATION
        public Event Event { get; set; }
        public Person Person { get; set; }
    }
}
