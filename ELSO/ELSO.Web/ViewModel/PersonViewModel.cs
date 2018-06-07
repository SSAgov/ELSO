using ELSO.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ELSO.Web.ViewModel
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Organization { get; set; }
        public string SignedInDate { get; set; }

        /// <summary>
        /// Takes an Person object and return PersonViewModel
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static PersonViewModel CreateViewModel(Person person)
        {
            var attendance = new List<Attendance>(person.Attendances);

            //var test = attendance != null ? attendance[0].ParticipatedDate.ToShortDateString() : string.Empty;
            if (person.FirstName != null)
            {
                int indexORG = person.FirstName.IndexOf("Contractor");
                if (indexORG != -1) { person.FirstName = person.FirstName.Remove(indexORG); }
            }
            return new PersonViewModel
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Phone = person.Phone,
                Organization = person.Organization,
                //SignedInDate = attendance?[0].ParticipatedDate.ToShortDateString()               
            };
        }
    }
}