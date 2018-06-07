using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ELSO.Model;

namespace ELSO.Data.Repositories
{
    /// <summary>
    /// Specific queries for events
    /// </summary>
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(dELSO3AEntities context)
            : base(context)
        {
        }
        /// <summary>
        /// Returns all events for an attendee ****Revisit this after model 3Achanges
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetByAttendee(int uid)
        {

            var events = new List<Event>();
          //var events = _context.Session.Include(s => s.Attendances)
          //        .Where(s => s.Attendances.Any(a => a.PersonId == uid && a.RoleCode.ToLower() == "PART"))
          //        .Select(s => s.Event)
          //        .Include(e => e.Patterns)
          //        .Include(a => a.Sessions);

            return events;
        }

        /// <summary>
        /// Get all events with dependencies
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IEnumerable<Event> GetAllEvents()
        {
            var events = _context.Pattern.Include(p => p.Sessions)
                         .Select(p => p.Event);

            return events;
        }

        /// <summary>
        /// Get an event by its unique id
        /// </summary>
        /// <param name="uid">event id</param>
        /// <returns>a single event</returns>
        public Event GetEventById(int uid)
        {
            var evnt = _context.Pattern.Include(p => p.Sessions)
                       .Where(p => p.Event.Id == uid)
                       .Select(p => p.Event)
                       .FirstOrDefault();
           // var sessions = evnt.Patterns.Select(x => x.Sessions).FirstOrDefault();
            //var attendance = sessions.Select(x => x.Attendances)

            return evnt;
        }


        /// <summary>
        /// Returns all events a person organized
        /// </summary>
        /// <param name="uid">Organizer uid</param>
        /// <returns></returns>
        public IEnumerable<Event> GetByOrganizers(int uid)
        {
          var events = _context.Session.Include(s => s.Attendances)
               .Where(s => s.Attendances.Any(a => a.PersonId == uid && a.RoleCode == "MDRT"))/*ToLower() == "mod"))*/
               .Select(s => s.Pattern).ToList().Select(x => x.Event).ToList();

            return events;
        }

        /// <summary>
        /// Get all attendees(participants) of a specific event
        /// </summary>
        /// <param name="uid"> event id</param>
        /// <returns></returns>
        public IEnumerable<Person> GetAttendees(int uid)
        {

            var attendees = (_context.Session.Include(s => s.Attendances)
               .Where(s => s.Pattern.EventId == uid)
               .SelectMany(s => s.Attendances))
               .Where(a => a.RoleCode == "PART")
               .Select(a => a.Person).Distinct();
            //var attendees  = from ea in _context.Attendance
            //                 join p in _context.Person on ea.PersonId equals p.Id
            //                 join s in _context.Session on ea.SessionId equals s.SessionId
            //                 where s.EventId == uid && ea.RoleCode == "PART"
            //                 orderby p.LastName, p.FirstName ascending
            //                 select p;

            return attendees.ToList();
        }

        /// <summary>
        /// Get all attendees(participants and moderator) of a specific event
        /// </summary>
        /// <param name="uid"> event id</param>
        /// <returns></returns>
        public IEnumerable<Person> GetAllAttendees(int uid)
        {
            var attendees = (_context.Session.Include(s => s.Attendances)
                  .Where(s => s.Pattern.EventId == uid)//&& s.Attendances.Any(a => a.RoleCode == "PART"))
                  .SelectMany(s => s.Attendances))
                  .Select(a => a.Person).Distinct();

            return attendees.ToList();
        }

        public IEnumerable<Person> GetRegisterees(int uid)
        {
            var registerees = (_context.Registration
                .Where(s => s.EventId == uid && s.RoleCode == "PART"))
                .Select(a => a.Person).Distinct();

            return registerees.ToList();
        }

        public IEnumerable<Person> GetAllRegisterees(int uid)
        {
            var registerees = (_context.Registration
                .Where(s => s.EventId == uid))
                .Select(a => a.Person).Distinct();

            return registerees.ToList();
        }

        public IEnumerable<Person> GetOrganizers(int uid)
        {

            var organizers = (_context.Session.Include(s => s.Attendances)
                .Where(s => s.Pattern.EventId == uid && s.Attendances.Any(a => a.RoleCode == "MDRT"))
                .SelectMany(s => s.Attendances))
                .Select(a => a.Person).Distinct();
            return organizers.ToList();
        }

        //Get All future/upcoming meetings
        public IEnumerable<Event> GetUpcomingMeetings()
        {
            var upcomingDate = DateTime.Now.AddDays(21);
            var upcoming = _context.Pattern.Include( p => p.Sessions)
                           .Where(p => p.EndDate >= DateTime.Now && p.EndDate <= upcomingDate)
                           .Select(a => a.Event).Distinct().ToList();

            return upcoming;
        }

        // Get All past meetings
        public IEnumerable<Event> GetPastMeetings()
        {
            var past = _context.Pattern.Include(p => p.Sessions)
                       .Where(p => p.EndDate < DateTime.Now)
                       .Select(a => a.Event).Distinct().ToList();
           
            return past;
        }

        public IEnumerable<Event> GetEventByDate(DateTime date)
        {
            var events = _context.Pattern.Include(p => p.Sessions)
                          .Where(p => p.StartDate == date.Date)
                          .Select(a => a.Event).Distinct().ToList();

            return events;
        }

        /// <summary>
        /// Return event registrants given an event id
        /// </summary>
        /// <param name="uid">event id</param>
        /// <returns>List of events</returns>
        public IEnumerable<Event> GetRegistrants(int uid)
        {
            var events = _context.Event
                .Where(e => e.Id == uid);

            return events;
        }

        /// <summary>
        /// given an event and person, this function crates an attendate record
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="personId"></param>
        /// <returns>true | false</returns>
        public bool SignIn(int eventId, int personId)
        {
            var PersonRep = new PersonRepository(_context);
            var evnt = GetEventById(eventId);
            var person = PersonRep.GetByID(personId);
            var session = evnt.Patterns.Select(x => x.Sessions).FirstOrDefault();
            var sessionId = session.Select(r => r.SessionId).FirstOrDefault();
            var currDate = DateTime.Now;
            if (evnt != null && person != null)
            {
                var att = new Attendance
                {
                    CreatedDate = currDate,
                    ParticipatedDate = currDate,
                    RoleCode = "PART",
                    // TODO : This is with the understanding that current an event only has one session.
                    SessionId = sessionId,
                    Person = person
                };

                _context.Attendance.Add(att);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        /// <summary>
        /// given an event and person, this function crates an registration record
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="personId"></param>
        /// <returns>true | false</returns>
        public string Register(int eventId, int personId, bool IsOrganizer)
        {
            var PersonRep = new PersonRepository(_context);
            var evnt = GetEventById(eventId);
            var person = PersonRep.GetByID(personId);
            var registerCount = evnt.Registrations.Count(s => s.RoleCode == "PART");
            var currDate = DateTime.Now;
            int eventRegCount = 0;
            var maxCount = new List<EventMaxCount>(evnt.EventMaxCount);
            if (maxCount.Count > 0)
            {
                eventRegCount = maxCount[0].eventMaxCount;
            }
            if (evnt != null && person != null)
            {
                var register = new Registration
                {
                    CreatedDate = currDate,
                    AssignmentDate = currDate,
                    RoleCode = "PART",
                    EventId = eventId,
                    Person = person
                };
                if (IsOrganizer || eventRegCount == 0 || eventRegCount > registerCount)
                {
                    _context.Registration.Add(register);
                    _context.SaveChanges();
                    return "";
                }
                else return "Registration maximum limit has been reached. Please contact meeting organizer";
            }
            return "Person/Event couldn't be found.";
        }

        /// <summary>
        /// given an event and person, this function deletes an registration record
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="personId"></param>
        /// <returns>true | false</returns>
        public string Unregister(int eventId, int personId)
        {
            var evnt = GetEventById(eventId);
            var register = evnt.Registrations.FirstOrDefault(s => s.PersonId == personId && s.RoleCode == "PART");
            if (register != null)
            {
                try
                {
                    // remove registration
                    _context.Registration.Remove(register);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return "";
            }
            else
            {
                return "User not registered";
            }
        }

        /// <summary>
        /// given an event and person, this function checks if user has already signed In
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="personId"></param>
        /// <returns>true | false</returns>
        /// 
        public string registrationCount(int eventId)
        {
            var evnt = GetEventById(eventId);
            if (evnt != null)
            {
                var registerCount = evnt.Registrations.Count(s => s.RoleCode == "PART");
                int eventRegCount = 0;
                var maxCount = new List<EventMaxCount>(evnt.EventMaxCount);
                if (maxCount.Count > 0)
                {
                    eventRegCount = maxCount[0].eventMaxCount;
                    if (eventRegCount == registerCount)
                    {
                        return "Registration is now closed. Please contact the Meeting Organizer for further help";
                    }
                }
                return "";
            }
            return "Event not found";

        }
        /// <summary>
        /// given an event and person, this function checks if user has already signed In
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="personId"></param>
        /// <returns>true | false</returns>

        public bool IsSignedIn(int eventId, int personId)
        {
            var PersonRep = new PersonRepository(_context);
            var evnt = GetEventById(eventId);
            var person = PersonRep.GetByID(personId);

            if (evnt != null && person != null)
            {
                return evnt.Patterns.Select(x => x.Sessions).FirstOrDefault().SelectMany(s => s.Attendances).Any(a => a.PersonId == personId && a.RoleCode == "PART");

            }

            return false;
        }

        public bool IsRegister(int eventId, int personId)
        {
            var PersonRep = new PersonRepository(_context);
            var evnt = GetEventById(eventId);
            var person = PersonRep.GetByID(personId);

            if (evnt != null && person != null)
            {
                return evnt.Registrations.Any(a => a.PersonId == personId && a.RoleCode == "PART");
            }

            return false;
        }
        public string deleteEvent(int eventId)
        {
            var evnt = _context.Pattern.Include(p => p.Sessions)
                      .Where(p => p.Event.Id == eventId)
                      .Select(p => p.Event)
                      .FirstOrDefault();
            //var evnt = _context.Session.Include(s => s.Attendances)
            //     .Where(s => s.Pattern.EventId == eventId)
            //     .Select(s => s.Event)
            //     .Include(e => e.Patterns)
            //     .Include(a => a.Sessions)
            //     .FirstOrDefault();

            if (evnt != null)
            {
                try
                {
                    var patterns = evnt.Patterns;
                    var sessions = evnt.Patterns.Select(x => x.Sessions).FirstOrDefault();
                    var attendance = sessions.SelectMany(a =>a.Attendances);
                    // remove attendances first
                    _context.Attendance.RemoveRange(attendance);
                    // remove Sessions
                    _context.Session.RemoveRange(sessions);
                    // remove patterns
                    _context.Pattern.RemoveRange(patterns);
                    //remove Registrations
                    var registration = evnt.Registrations;
                    _context.Registration.RemoveRange(registration);
                    //remove EventMaxCount
                    var eventCount = evnt.EventMaxCount;
                    _context.EventMaxCount.RemoveRange(eventCount);
                    // remove Event
                    _context.Event.Remove(evnt);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return "Success";
            }
            else
            {
                return "Event cannot be deleted";
            }
        }
    }
}
