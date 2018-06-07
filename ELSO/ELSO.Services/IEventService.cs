using System;
using System.Collections.Generic;
using ELSO.Model;

namespace ELSO.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetEventByDate(DateTime date);
        IEnumerable<Event> GetPastEvents();
        IEnumerable<Event> GetEventByPerson(int uid);
        IEnumerable<Person> GetAllRegisterees(int uid);
        Event GetEventByUID(int uid);
        void AddPersonToEvent(int personUID, int eventUID);
        void Save(Event evnt);
        bool IsSignedIn(int eventId, int personId);
        bool SignIn(int eventId, int personId);
        string deleteEvent(int eventId);
    }
}
