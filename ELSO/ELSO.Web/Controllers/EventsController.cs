using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ELSO.Services;
using ELSO.Model;
using ELSO.Web.ViewModel;
using System.Net.Http.Formatting;

namespace ELSO.Web.Controllers
{
    public class EventsController : ApiController
    {
        private EventService _service;
        public EventsController()
        {
            _service = new EventService();
        }
        // GET: api/Events
        public HttpResponseMessage Get()
        {
           IEnumerable<EventViewModel> eventVMs = null;   
                    
            try
            {
               eventVMs = _service.GetAll().Select(a => EventViewModel.CreateViewModel(a)).ToList<EventViewModel>().OrderByDescending(a => a.StartDate);

               return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = eventVMs.ToArray()
                });

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,"Error retrieving events");
            }
         
        }

  

        [Route("api/Events/PastEvents/"),
        HttpGet]
        public HttpResponseMessage PastEvents()
        {
           IEnumerable<EventViewModel> pastEvents = null;
            try
            {
                 pastEvents = _service.GetPastEvents().Select(a => EventViewModel.CreateViewModel(a)).ToList<EventViewModel>().OrderByDescending(a =>a.StartDate);

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = pastEvents.ToArray()
                });
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error retrieving events");
            }
            // return Request.CreateResponse(HttpStatusCode.OK, meetings);

           
        }

        [Route("api/Events/UpcomingMeetings/"),
       HttpGet]
        public HttpResponseMessage UpcomingMeetings()
        {
            IEnumerable<EventViewModel> upcomingEvents = null;
            try
            {
               upcomingEvents = _service.UpcomingMeetings().Select(a => EventViewModel.CreateViewModel(a)).ToList<EventViewModel>().OrderBy(a => a.StartDate);
                
              return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = upcomingEvents.ToArray()
                });
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error retrieving events");
            }
            //return Request.CreateResponse(HttpStatusCode.OK, meetings);
           
        }
        
        [Route("api/Events/GetEventByOrganizer/"),
     HttpGet]
        public HttpResponseMessage GetEventByOrganizer()
        {
             IEnumerable<EventViewModel> events = null;
            var personService = new PersonService();
            try
            {
                var pin = User.Identity.Name.Split('\\')[1];
                var person = personService.getUserInfobyPIN(pin);
                if (person != null)
                {
                    events = personService.GetOrganizerEvents(person.Id).Select(a => EventViewModel.CreateViewModel(a)).ToList<EventViewModel>().OrderByDescending(a => a.StartDate);
                }
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data= events.ToArray()
                });

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error retrieving events");
            }

        }

        // GET: api/Events/5
        public HttpResponseMessage Get(int id)
        {
            // Event evnt = null;
            EventViewModel evnt = null;
            if (id <= 0)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "id must be greater than zero");
           try
            {
               // evnt = _service.GetEventByUID(id);
             evnt = EventViewModel.CreateViewModel(_service.GetEventByUID(id));
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Error retrieving event");
            }

            return Request.CreateResponse(HttpStatusCode.OK,evnt);
        }

        [HttpPost,
            Route("api/")]
        public HttpResponseMessage AddOrganizer(int eventId, string pin)
        {
            try
            {
                if(!string.IsNullOrEmpty(pin) && eventId != 0)
                {
                    var personService = new PersonService();
                    var person = personService.getUserInfobyPIN(pin);
                }
                return Request.CreateResponse(HttpStatusCode.Created, "Success");
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to add organizer");
            }
        }

        // POST: api/Events
        [HttpPost,
            Route("api/Events/")]

        public HttpResponseMessage Post(EventViewModel evnt)
        {
            if (evnt.EventEndDate == null)
            {
                evnt.EventEndDate = evnt.EventStartDate;
            }
            if (Convert.ToDateTime(evnt.StartDate) < DateTime.Now)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Cannot create meeting for a past date");
            }
            if (evnt.EndDate <= evnt.StartDate)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Cannot create meeting for invalid end time.");
            }
            var personService = new PersonService();
            var genericService = new LKUPService();
            var startDate = Convert.ToDateTime($"{evnt.EventStartDate} {evnt.EventStartTime}");
            var endDate = Convert.ToDateTime($"{evnt.EventEndDate} {evnt.EventEndTime}");
            List<Attendance> atts = new List<Attendance>();
            List<Registration> registers = new List<Registration>();
            Attendance att =null;
            Registration regs = null;
            List<EventMaxCount> eventMaxCount = null;
            try
            {
                var pin = User.Identity.Name.Split('\\')[1];
                evnt.Organizers = evnt.Organizers ?? new List<string>();                
                evnt.Organizers.Add(pin);
                foreach (var organizerPin in evnt.Organizers)
                {
                    if (organizerPin != null)
                    {
                        var person = personService.getUserInfobyPIN(organizerPin);
                        // Create pattern
                        att = new Attendance { PersonId = person.Id, RoleCode = "MDRT", CreatedDate = DateTime.Now };
                        if (evnt.IsRegistration == true)
                        {
                            regs = new Registration { PersonId = person.Id, RoleCode = "MDRT", EventId = evnt.Id, AssignmentDate = DateTime.Now };
                            registers.Add(regs);
                        }
                        atts.Add(att);
                    }
                }
                /**** Pre-registration Begin ****/
               /* if (evnt.Participants != null)
                {
                    foreach (var attendeePin in evnt.Participants)
                    {
                        if (attendeePin != null)
                        {
                            var person = personService.GetPersonByPin(attendeePin);

                            if (person == null)
                            {
                                person = personService.GetFromGAL(attendeePin, person?.Email);
                                //TODO: Research on the usage of LGCY_ADMSTR_SWTCH Column
                                if (person.Phone != null)
                                {
                                    person.Phone = person.Phone.Replace("Black berry", "BB");
                                    person.Phone = person.Phone.Substring(0, Math.Min(35, person.Phone.Length));
                                }
                                personService.Save(person);
                            }
                            // Create pattern
                            regs = new Registration { PersonId = person.Id, RoleCode = "PART", EventId = evnt.Id, AssignmentDate = DateTime.Now };
                            registers.Add(regs);
                        }
                    }
                }*/
                /**** Pre-registration End ****/
                if(Convert.ToInt16(evnt.AttdMxCount) > 0)
                {
                    eventMaxCount = new List<EventMaxCount> { new EventMaxCount { RoleCode = "MDRT", eventMaxCount = Convert.ToInt16(evnt.AttdMxCount), CreatedDate = DateTime.Now } };
                 
                }
                // TODO: Null check before adding to pattern because it will create a constraint error
                var rType = genericService.GetPatternByDesc("One session, no recurrence") ?? new PatternType { PatternDescp = "One session, no recurrence" };
                var Sess = new Session
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Location = evnt.Location,
                    OriginalStartDate = startDate,
                    Attendances = atts
                };
                var patt = new Pattern { StartDate = startDate, PatternCount = 1, PatternStartCount = 1, PatternTypeId = rType.Id,
                    Sessions = new List<Session> {
                         Sess  },
                    CreatedDate = DateTime.Now, EndDate = endDate };
               

                // Create Event
                var newEvent = new Event
                {
                    EventName = evnt.EventName,
                    ETypeCode = "MTMG",
                    //Sessions = new List<Session> {
                    //        Sess
                    //    },
                    Registrations = registers,
                    EventMaxCount = eventMaxCount,
                    Patterns = new List<Pattern> { patt }, CreatedDate = DateTime.Now,
                   };

                _service.Save(newEvent);

                return Request.CreateResponse(HttpStatusCode.Created, "Success");

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to create event");
            }

        }


       [Route("api/Events/GetAttendees/")]
        [HttpGet]
        public HttpResponseMessage GetAttendees(int id)
        {
              IEnumerable <PersonViewModel> people = null;
            try
            {
               people = _service.GetAttendees(id).Select(a => PersonViewModel.CreateViewModel(a)).ToList<PersonViewModel>().OrderBy(a => a.FirstName);
            
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Error retrieving event");
            }
            
            return Request.CreateResponse(HttpStatusCode.OK, new { data = people.ToArray() });
        }

        [Route("api/Events/GetAllAttendees/")]
        [HttpGet]
        public HttpResponseMessage GetAllAttendees(int id)
        {
            IEnumerable<PersonViewModel> people = new List<PersonViewModel>();
            
            try
            {
                people = _service.GetAllAttendees(id).Select(a => PersonViewModel.CreateViewModel(a)).OrderBy(a => a.FirstName).ToList<PersonViewModel>();

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Error retrieving event");
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { data = people });
        }

        [Route("api/Events/GetRegisterees/")]
        [HttpGet]
        public HttpResponseMessage GetRegisterees(int id)
        {
            IEnumerable<PersonViewModel> people = new List<PersonViewModel>();
            try
            {
               // people = _service.GetRegisterees(id).Select(a => PersonViewModel.CreateViewModel(a)).ToList<PersonViewModel>();
              people = _service.GetRegisterees(id).Select(a => PersonViewModel.CreateViewModel(a)).ToList<PersonViewModel>().OrderBy(a => a.FirstName);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Error retrieving event");
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { data = people });
        }


        [Route("api/Events/GetAllRegisterees/")]
        [HttpGet]
        public HttpResponseMessage GetAllRegisterees(int id)
        {
            IEnumerable<PersonViewModel> people = new List<PersonViewModel>();
            try
            {
                 people = _service.GetAllRegisterees(id).Select(a => PersonViewModel.CreateViewModel(a)).OrderBy(a => a.FirstName).ToList<PersonViewModel>();
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Error retrieving event");
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { data = people });
        }

        // PUT: api/Events/5

        public void Put(int id, [FromBody]string value)
        {
           
        }

    }
}
