using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELSO.Model
{
    public class EventMetaData : BaseEntity
    {
        public int EventDateId { get; set; }       
      
        public int Occurrence { get; set; }
        public int RPT_Year { get; set; }
        public int RPT_Month { get; set; }
        public int RPT_Week { get; set; }
        public int RPT_Day { get; set; }
        [Required]
        [StringLength(5)]
        // Monday=m, Tuesday=t, Wednesday=w, Thrusday=h,Friday=f
        public string RPT_WeekDay { get; set; }
        public int DayOfMonth { get; set; }

        // NAVIGATION
        public Event EventDate { get; set; }
        public RecurrenceType Recurrence { get; set; }
    }

    public enum RecurrenceType
    {
        Daily, Weekly, Monthly, Yearly
    }
}
