using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ELSO.Model;
using ELSO.Data;
using ELSO.Data.Repositories;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;

namespace ELSO.Services
{
    public class PersonService : IPersonService
    {
        private UnitOfWork _uow;
        public PersonService()
        {
            _uow = new UnitOfWork();
        }
        /// <summary>
        /// Get a person by their SSA PIN
        /// </summary>
        /// <param name="pin"></param>
        /// <returns></returns>
        public Person GetPersonByPin(string pin)
        {
            return _uow.PeopleRepository.Get(a => a.SSA_PIN == pin).FirstOrDefault();
        }
        /// <summary>
        /// Returns every record from database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Person> GetAll()
        {
            return _uow.PeopleRepository.Get();
        }
        /// <summary>
        /// Given an email, return the person's UID
        /// </summary>
        /// <param name="email"> Email </param>
        /// <returns> interger = UID</returns>
        public Person GetByEmail(string email)
        {
            return _uow.PeopleRepository.Get(a => a.Email.ToLower() == email.ToLower()).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        /// <summary>
        /// Creates/updates a person 
        /// </summary>
        /// <param name="person"></param>
        public void Save(Person person)
        {
            if (person.Id > 0)
                _uow.PeopleRepository.Update(person);
            else
            {
                if (person.Phone != null)
                {
                    person.Phone = person.Phone.Replace("Black berry", "BB");
                    person.Phone = person.Phone.Substring(0, Math.Min(35, person.Phone.Length));
                }
                if (person.FirstName != null)
                {
                    var index = person.FirstName.IndexOf("Contractor");
                    if (index != -1) { person.FirstName = person.FirstName.Remove(index); }
                }
                _uow.PeopleRepository.Insert(person);
            }
            try {
                _uow.Save(); }
            catch (Exception ex)
            {
                var test = "";
            }
        }

        public Person GetFromGAL(string Pin=null, string Email = null)
        {
            return _uow.PeopleRepository.GetFromGAL(Pin,Email);
        }

        public IEnumerable<Event> GetOrganizerEvents(int OrganizerID)
        {
            return _uow.EventRepository.GetByOrganizers(OrganizerID);
        }

        public Person getUserInfobyPIN(string pin)
        {
            Person person = null;
            Person personDB = null;
            try
            {
                person = GetFromGAL(pin) ?? getUserfromAD(pin);
                if (person != null)
                {
                    personDB = _uow.PeopleRepository.Get(a => a.SSA_PIN == pin).FirstOrDefault();
                    if (personDB != null && (person.SSA_PIN == personDB.SSA_PIN))
                    {
                        if (person.Organization != personDB.Organization)
                        {
                            personDB.Organization = personDB.Organization;
                        }
                        if (person.FirstName != personDB.FirstName)
                        {
                            personDB.FirstName = personDB.FirstName;
                        }
                        if (person.LastName != personDB.LastName)
                        {
                            personDB.LastName = person.LastName;
                        }
                        if (person.Phone != personDB.Phone)
                        {
                            personDB.Phone = person.Phone;
                        }
                        if (person.Email != personDB.Email)
                        {
                            personDB.Email = person.Email;
                        }
                        Save(personDB);
                        return personDB;
                    } else
                    {
                        Save(person);
                        return person;
                    }
                }
             }
            catch (Exception ex)
            {
                throw ex;
            }
            return person;

        }
        public Person getUserInfobyEmail(string email)
        {
            Person person = null;
            Person personDB = null;
            try
            {
               if (email.EndsWith("[EMAIL_ADDRESS]"))
                    {
                        person = GetFromGAL(null, email) ?? getUserfromAD(null, email);
                        personDB = GetByEmail(email);
                       if (personDB != null) // update all the fields
                        {
                            if (person.SSA_PIN != personDB.SSA_PIN)
                            {
                                personDB.SSA_PIN = personDB.SSA_PIN;
                            }
                            if (person.Organization != personDB.Organization)
                            {
                                personDB.Organization = personDB.Organization;
                            }
                            if (person.FirstName != personDB.FirstName)
                            {
                                personDB.FirstName = personDB.FirstName;
                            }
                            if (person.LastName != personDB.LastName)
                            {
                               personDB.LastName = person.LastName;
                            }
                            if (person.Phone != personDB.Phone)
                            {
                                personDB.Phone = person.Phone;
                            }
                             Save(personDB);
                             return personDB;
                       } else if(personDB == null && person?.Id == 0){
                                Save(person);
                                return person;
                            }
                   } else  person = GetByEmail(email);
             
             return person;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public Person getUserfromAD(string pin = null, string emailAddress = null)
        {
            Person person = new Person();
            try
            {
                //Get production Domains --- Start
                Forest adForest = Forest.GetForest(new DirectoryContext(DirectoryContextType.Forest, "[SERVERNAME]"));
                DirectoryEntry de = adForest.RootDomain.GetDirectoryEntry();
                var oDomains = adForest.Domains;
                foreach (var domain in oDomains)
                {
                    string domainFirstNode = domain.ToString().Substring(0, domain.ToString().IndexOf('.'));
                    // create your domain context
                    using (PrincipalContext ctx = new PrincipalContext((ContextType.Domain), domainFirstNode))
                    {
                        // define a "query-by-example" principal - 
                        UserPrincipal qbeUser = new UserPrincipal(ctx);
                        if (emailAddress != null) { qbeUser.EmailAddress = emailAddress; }
                        if (pin != null) { qbeUser.SamAccountName = pin; }
                        // create your principal searcher passing in the QBE principal    
                        PrincipalSearcher search = new PrincipalSearcher(qbeUser);
                        // find all matches
                        foreach (var foundUser in search.FindAll())
                        {
                            if (foundUser != null)
                            {
                                DirectoryEntry directoryEntry = foundUser.GetUnderlyingObject() as DirectoryEntry;
                                PropertyCollection props = directoryEntry.Properties;
                                person.FirstName = directoryEntry.Properties["givenName"].Value.ToString();
                                person.LastName = directoryEntry.Properties["sn"].Value.ToString();
                                person.Phone = directoryEntry.Properties["telephoneNumber"].Value.ToString();
                                person.Email = directoryEntry.Properties["mail"].Value.ToString();
                                person.Organization = directoryEntry.Properties["company"].Value.ToString();
                                person.SSA_PIN = directoryEntry.Properties["sAMAccountName"].Value.ToString();
                                return person;
                            }
                        }
                    }
                }
            }
       
            catch (Exception ex)
            {
                return null;
            }
            return person;
        }

        public bool isAuthenticated(string PIN, string domain)
        {
            try
            {
                // set up domain context
                using (PrincipalContext ctx = new PrincipalContext((ContextType.Domain), domain))
                {
                    // find user by display name
                    UserPrincipal ADUser = UserPrincipal.FindByIdentity(ctx, PIN);
                    // 
                    if (ADUser != null)
                    {
                        // get the user's groups
                        var groups = ADUser.GetAuthorizationGroups();
                        foreach (GroupPrincipal group in groups)
                        {
                            if (group.Name == "ELSO Administrators")
                                return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
