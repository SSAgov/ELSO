using ELSO.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ELSO.Web.ViewModel
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventStartDate { get; set; }

        public bool? IsRegistration { get; set; }

        public string meetingType { get; set; }
        /// <summary>
        /// Returns a datetime object representation of the eventstartdate string 
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                if (!string.IsNullOrEmpty(EventStartDate))
                {
                    return Convert.ToDateTime($"{EventStartDate} {EventStartTime}");
                }
                else
                    return null;
            }
        }
        public string EventStartTime { get; set; }
       public string EventEndTime { get; set; }
        /// <summary>
        /// Returns a datetime object presentation of the eventenddate string 
        /// </summary>
        public DateTime? EndDate {
            get
            {
                if (!string.IsNullOrEmpty(EventEndDate))
                {
                    return Convert.ToDateTime($"{EventEndDate} {EventEndTime}");
                }
                else
                    return null;              
            }
        }
        public string EventEndDate { get; set; }
         public string Location { get; set; }
        public List<string> Organizers { get; set; }
        public List<string> Registrations { get; set; }
        public string currentRegCount { get; set; }
        public string AttdMxCount { get; set; }
        public string CreatedByPIN { get; set; }
        //public string EventType { get; set; }
        //public ICollection<Registration> Registrations { get; set; }
        //public ICollection<Attendance> Attendances { get; set; }
        //public ICollection<Session> Sessions { get; set; }

        /// <summary>
        ///// Takes a EventViewModel and create a Event object
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        //public static Event CreateModel(EventViewModel vm)
        //{
        //    return new Event
        //    {
        //        EventId = vm.Id, 
        //        Registrations = 
        //        Location = vm.Location,
        //        EventName = vm.EventName,
        //        EventEndDate = Convert.ToDateTime(vm.EventEndDate),
        //        EventStartDate = Convert.ToDateTime(vm.EventStartDate)
        //    };
        //}

        /// <summary>
        /// Takes an event object and return EventViewModel
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static EventViewModel CreateViewModel(Event evnt)
        {
            // TODO : this current code is with the understand that there will only be on session and one pattern per events
        
            var patterns = new List<Pattern>(evnt.Patterns);
            var session = new List<Session>(patterns[0].Sessions);
            var attendees = new List<Attendance>(session[0].Attendances);
            var registrants = new List<Registration>(evnt.Registrations);
            var maxCount = new List<EventMaxCount>(evnt.EventMaxCount);
            var currRegCount = evnt.Registrations.Count;
            int registrationCount =0 ;
            // check if is registration
            var registerees = new List<string>();
            foreach (var regs in registrants)
            {
                if (regs.RoleCode.ToLower() == "part")
                {
                    registerees.Add(regs.Person.SSA_PIN);
                }
            }
            var isRegistration = currRegCount > 0 ? true : false;
            if (maxCount.Count>0)
            {
                //  TODO : 
                registrationCount = maxCount[0].eventMaxCount;
            }
            //check for participants
            var organizers = new List<string>();
            foreach (var att in attendees)
            {
                if (att.RoleCode.ToLower() == "mdrt")
                {
                    organizers.Add(att.Person.SSA_PIN);
                }
                //else
                //    Participants.Add(att.Person.SSA_PIN);

            }

            return new EventViewModel
            {
                Id = evnt.Id,
                EventEndDate = patterns[0].EndDate.Value.ToShortDateString(),
                EventEndTime = patterns[0].EndDate.Value.ToShortTimeString(),
                EventStartDate = patterns[0].StartDate.ToShortDateString(),
                EventStartTime = patterns[0].StartDate.ToShortTimeString(),
                Location = session[0].Location,
                EventName = evnt.EventName,
                Organizers = organizers,
                IsRegistration = isRegistration,
                AttdMxCount = registrationCount.ToString(),
                currentRegCount = registerees.Count.ToString()
                //CreatedByPIN = attendees[0].Person.
            };
        }
    }
}