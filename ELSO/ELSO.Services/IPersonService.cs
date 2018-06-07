using ELSO.Model;
using System.Collections.Generic;

namespace ELSO.Services
{
    public interface IPersonService
    {
        /// <summary>
        /// Retrieve a person by their SSA Pin
        /// </summary>
        /// <param name="pin">SSA PIN</param>
        /// <returns>a person object</returns>
        Person GetPersonByPin(string pin);
        /// <summary>
        /// Retrieves a person by their email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> a Person </returns>
        Person GetByEmail(string email);
        /// <summary>
        /// Retrieve all record from the table
        /// </summary>
        /// <returns>List of People</returns>
        IEnumerable<Person> GetAll();

        IEnumerable<Event> GetOrganizerEvents(int OrganizerID);
        void Save(Person person);
    }
}