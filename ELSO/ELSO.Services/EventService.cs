using ELSO.Data;
using ELSO.Data.Repositories;
using ELSO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ELSO.Services
{
    public class EventService : IEventService
    {
        private UnitOfWork _uow;
        public EventService()
        {
            _uow = new UnitOfWork();
        }

        public IEnumerable<Event> GetEventByDate(DateTime date)
        {
            return _uow.EventRepository.GetEventByDate(date);
        }

        public IEnumerable<Event> GetEventByPerson(int uid)
        {
            return _uow.EventRepository.GetByAttendee(uid);
        }

        public void Save(Event evnt)
        {
            try
            {
                if (evnt.Id > 0)
                {
                    _uow.EventRepository.Update(evnt);
                }
                else
                {
                    _uow.EventRepository.Insert(evnt);
                }
                _uow.Save();
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<Event> UpcomingMeetings()
        {

            return _uow.EventRepository.GetUpcomingMeetings();

        }
        /// <summary>
        /// Returns every record from database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Event> GetAll()
        {
            return _uow.EventRepository.GetAllEvents();
        }
        /// <summary>
        /// Return an event by on the UID
        /// </summary>
        /// <param name="uid"> event UID</param>
        /// <returns>a event object</returns>

        public Event GetEventByUID(int uid)
        {
            return _uow.EventRepository.GetEventById(uid);
        }

        /// <summary>
        /// Returns past events
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Event> GetPastEvents()
        {
            return _uow.EventRepository.GetPastMeetings();
        }

        public IEnumerable<Person> GetAttendees(int uid)
        {
            return _uow.EventRepository.GetAttendees(uid);
        }

        public IEnumerable<Person> GetAllAttendees(int uid)
        {
            return _uow.EventRepository.GetAllAttendees(uid);
        }

        public IEnumerable<Person> GetRegisterees(int uid)
        {
            return _uow.EventRepository.GetRegisterees(uid);
        }

        public IEnumerable<Person> GetAllRegisterees(int uid)
        {
            return _uow.EventRepository.GetAllRegisterees(uid);
        }

        public void AddPersonToEvent(int personUID, int eventUID)
        {
            throw new NotImplementedException();
        }

        public bool SignIn(int eventId, int personId)
        {
            return _uow.EventRepository.SignIn(eventId, personId);

        }

        public bool IsSignedIn(int eventId, int personId)
        {
            return _uow.EventRepository.IsSignedIn(eventId, personId);

        }
        public bool IsRegister(int eventId, int personId)
        {
            return _uow.EventRepository.IsRegister(eventId, personId);

        }

        public string Register(int eventId, int personId,bool IsOrganizer)
        {
            return _uow.EventRepository.Register(eventId, personId,IsOrganizer);

        }

        public string Unregister(int eventId, int personId)
        {
            return _uow.EventRepository.Unregister(eventId, personId);
        }

        public string registrationCount(int eventId)
        {
            return _uow.EventRepository.registrationCount(eventId);
        }

        public string deleteEvent(int eventId)
        {
            return _uow.EventRepository.deleteEvent(eventId);

        }

    }
}
