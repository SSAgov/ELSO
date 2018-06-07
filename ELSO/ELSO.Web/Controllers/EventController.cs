using ELSO.Model;
using ELSO.Services;
using System;
using System.Web.Mvc;
using ELSO.Web.ViewModel;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net;

namespace ELSO.Web.Controllers
{
    public class EventController : Controller
    {
        private EventService _service;
        private PersonService _pservice;
        public EventController()
        {
            _service = new EventService();
            _pservice = new PersonService();
        }
   
         // GET: Event
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Past()
        {
            return View();
        }

             [HttpGet]
        // GET: Event/Details/id
        public ActionResult SignIn(int id)
        {
            var userService = new ELSO.Services.PersonService();
            var pin = User.Identity.Name.Split('\\')[1];
            var domain= User.Identity.Name.Split('\\')[0];
            var user = userService.getUserInfobyPIN(pin);
            if (user.FirstName != null)
            {
                var index = user.FirstName.IndexOf("Contractor");
                if (index != -1) { user.FirstName = user.FirstName.Remove(index); }
            }
            var name = user.FirstName + " " + user.LastName;
            ViewBag.Name = name;
            ViewBag.Pin = pin;
            bool IsOrganizer = false;
            bool allowSignIn = false;
            Person userOrg = null;
           string organizers = null;
          
            EventViewModel evnt = new EventViewModel();
            Event evntModel = _service.GetEventByUID(id);
            if (evntModel == null)
            {
                return View("RedirectInfo");
            }
            evnt = EventViewModel.CreateViewModel(_service.GetEventByUID(id));
            if (userService.isAuthenticated(pin, domain))
            {
                IsOrganizer = true;
            }
            foreach (var organizer in evnt.Organizers)
            {
                if (!IsOrganizer)
                {
                    IsOrganizer = (pin == organizer) ? true : false;
                }
                userOrg = userService.getUserInfobyPIN(organizer);
                if (userOrg.FirstName != null)
                {
                    int indexORG = userOrg.FirstName.IndexOf("Contractor");
                    if (indexORG != -1) { userOrg.FirstName = userOrg.FirstName.Remove(indexORG); }
                }
                string organizerName = userOrg.FirstName + " " + userOrg.LastName;
                organizers+=String.Join(", ", organizerName)+", ";
            }
            organizers = organizers.Substring(0, organizers.Length - 2);
          
            DateTime checkEndDate = Convert.ToDateTime(evnt.EndDate).AddDays(1);
            if(checkEndDate > DateTime.Now)
            {
               allowSignIn = true;
            }
            ViewBag.allowSignIn = allowSignIn;
            ViewBag.IsOrganizer = IsOrganizer;
            ViewBag.organizers = organizers;
            // TODO : check id once data schema is complete, for now we will return an empty event even if none is found

            //ModelState.AddModelError("", "No events found, please try again or contact the event coordinator");
            return View(evnt);
    }
        public ActionResult UpComing()
        {
            return View();
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            var userService = new ELSO.Services.PersonService();
            var pin = User.Identity.Name.Split('\\')[1];
            var user = userService.getUserInfobyPIN(pin);
            if (user.FirstName != null)
            {
                var index = user.FirstName.IndexOf("Contractor");
                if (index != -1) { user.FirstName = user.FirstName.Remove(index); }
            }

            var name = user.FirstName + " " + user.LastName;
            ViewBag.Name = name;
            ViewBag.Pin = pin;
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel evnt)
        {
            var personService = new PersonService();
            evnt.EventStartDate =($"{evnt.EventStartDate} {Request["startTime"]}");
            evnt.EventEndDate =($"{evnt.EventEndDate} {Request["endTime"]}");

            try
            {
                var pin = User.Identity.Name.Split('\\')[1];
                Person person = personService.getUserInfobyPIN(pin);
                evnt.CreatedByPIN = person.SSA_PIN;
                ModelState.AddModelError("", "You have Successfully create an event");
             }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sorry, we were unable to create your event");
            }
           
            return View(evnt);
        }

        [HttpPost]

        public JsonResult addGuest(Person person, String evntId)
        {
            string formError = string.Empty;
            var personService = new PersonService();
            try
            {
                if (person != null && person?.Id == 0)
                {
                    if (person.Phone != null)
                        {
                            person.Phone = person.Phone.Substring(0, Math.Min(35, person.Phone.Length));
                        }
                        personService.Save(person);
                }
            }
            catch (Exception ex)
            {
                formError = "Cannot add the person";
            }
            return Json(
              new
              {
                  status = string.IsNullOrEmpty(formError) ? "success" : "error",
                  message = formError, personId = person.Id
              });
        }

        [HttpPost]
        public JsonResult SignIn(String personId, String evntId)
        {
            string formError = string.Empty;
             try
            {
                if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(evntId))
                {
                    int prsnId = Convert.ToInt32(personId);
                    int eventId = Convert.ToInt32(evntId);
                    // validate and fetch event
                    if ((prsnId > 0) && evntId.All(char.IsNumber))
                    {
                      EventViewModel evnt = EventViewModel.CreateViewModel(_service.GetEventByUID(eventId));
                       if (evnt != null)
                        {
                            if (_service.IsSignedIn(eventId, prsnId))
                            {
                                formError = "User already signed in";

                            } else if (!_service.SignIn(eventId, prsnId))
                            {
                                formError = "Unable to sign in";
                            }
                        }
                        else
                            formError = "Cannot find the event specified, please try again and make sure you have a valid event";
                    }
                    else
                        formError = "Person couldn't be found";
                }
                else
                    formError = "UserID field cannot be empty";            

            }
            catch(Exception ex)
            {
                formError = "Oops, something went wrong";
            }
            return Json(
                new
                {
                    status = string.IsNullOrEmpty(formError) ? "success" : "error",
                    message = formError
                });
        }
        [HttpPost]
        public JsonResult Register(String personId, String Id)
        {
            string formError = string.Empty;
            var pin = User.Identity.Name.Split('\\')[1];
            bool IsOrganizer = false;
            try
            {
                if (!string.IsNullOrEmpty(personId) && !string.IsNullOrEmpty(Id))
                {
                    int prsnId = Convert.ToInt32(personId);
                    int eventId = Convert.ToInt32(Id);
                    // validate and fetch event
                    if ((prsnId > 0) && Id.All(char.IsNumber))
                    {
                        EventViewModel evnt = EventViewModel.CreateViewModel(_service.GetEventByUID(eventId));
                        foreach (var organizerPin in evnt.Organizers)
                        {
                            if (!IsOrganizer)
                            {
                                IsOrganizer = (pin == organizerPin) ? true : false;
                            }
                        }
                        if (evnt != null)
                        {
                            if (_service.IsRegister(eventId, prsnId))
                            {
                                formError = "User already Registered";
                            }
                            else
                                formError = _service.Register(eventId, prsnId, IsOrganizer);
                           }
                        else
                            formError = "Cannot find the event specified, please try again and make sure you have a valid event";
                    }
                    else
                        formError = "Person couldn't be found";
                }
                else
                    formError = "UserID field cannot be empty";

            }
            catch (Exception ex)
            {
                formError = "Oops, something went wrong";
            }
            return Json(
                new
                {
                    status = string.IsNullOrEmpty(formError) ? "success" : "error",
                    message = formError
                });
        }

        public JsonResult Unregister(String inputValue, String Id)
        {
            string formError = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(inputValue) && !string.IsNullOrEmpty(Id))
                {
                    var personService = new PersonService();
                    Person person = null;
                    if (inputValue.All(char.IsNumber) && inputValue.Length == 6)
                        person = personService.getUserInfobyPIN(inputValue);
                    // TODO : Not sure if this is necessary. 
                    else if (inputValue.ToLower().Contains("@"))
                        person = personService.getUserInfobyEmail(inputValue);
                    if ((person?.Id > 0) && Id.All(char.IsNumber))
                    {
                        formError = _service.Unregister(Convert.ToInt32(Id), Convert.ToInt32(person.Id));
                    }
                    else
                    {
                        formError =  "Event or Person couldn't be found";
                    }
                }
            }
            catch (Exception ex)
            {
                formError = "Exception occured";
            }

            return Json(
               new
               {
                   status = string.IsNullOrEmpty(formError) ? "success" : "error",
                   message = formError
               });
        }

        //String Parsed Methods

        private string ParsedString(string input)
        {
            String tstStr =  new String(input.Where(c => char.IsLetter(c) || char.IsDigit(c)).ToArray());
            return tstStr;
        }
        // GET: Event/Edit/5
        public JsonResult Edit(EventViewModel evnt)
        {
            var evntId = evnt.Id;
            var personService = new PersonService();
            DateTime startDate = Convert.ToDateTime(evnt.EventStartDate + " " + evnt.EventStartTime);
            DateTime endDate = Convert.ToDateTime(evnt.EventEndDate + " " + evnt.EventEndTime);
            try
            {
                var pin = User.Identity.Name.Split('\\')[1];
                var person = personService.getUserInfobyPIN(pin);
                var oldEvent = _service.GetEventByUID(evntId);

                oldEvent.EventName = evnt.EventName;
               foreach (var patt in oldEvent.Patterns)
                {
                    patt.StartDate = startDate;
                    patt.EndDate = endDate;
                    foreach (var sess in patt.Sessions)
                    {
                        sess.StartDate = startDate;
                        sess.EndDate = endDate;
                        sess.Location = evnt.Location;
                    }
                }

                 _service.Save(oldEvent);

                return Json("Event Updated", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
    }
}

        // GET: Event/Delete/5
        public JsonResult Delete(int id)
        {
            _service.deleteEvent(id);
            return Json("Success",JsonRequestBehavior.AllowGet);
        }

    }
}
