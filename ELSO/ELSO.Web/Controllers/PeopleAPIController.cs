using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ELSO.Services;
using ELSO.Model;
using System.Net.Mail;

namespace ELSO.Web.Controllers
{
    public class PeopleAPIController : ApiController
    {
        private PersonService _service;

        public PeopleAPIController()
        {
            _service = new PersonService();
        }
        // GET: api/PeopleAPI
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //public HttpResponseMessage Get(int id)
        //{
        //    return null;
        //}

        // GET: api/PeopleAPI/5
        public HttpResponseMessage GetUserInfo(string pinEmail)
        {

            Person person = new Person();
            try
            {
                if (!string.IsNullOrEmpty(pinEmail)) {
                    if (pinEmail.All(char.IsNumber) && pinEmail.Length == 6)
                    {
                        person = _service.getUserInfobyPIN(pinEmail);
                        if (person?.SSA_PIN == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter a valid SSA PIN");
                        }
                    }
                    else if (pinEmail.ToLower().Contains("@") && IsAnEmailAddress(pinEmail))
                    {
                        person = _service.getUserInfobyEmail(pinEmail);
                        if (person?.Email == null)
                        {
                            if (pinEmail.EndsWith("[EMAIL_ADDRESS]"))  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter a valid SSA Email Address");
                            else return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Person Couldn't be found");
                        }
                    }
                    else { return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter a valid email address or SSA PIN"); }
                    return Request.CreateResponse(HttpStatusCode.OK, new { FirstName = person.FirstName, LastName = person.LastName, SSA_Pin = person.SSA_PIN, Organization = person.Organization, Email = person.Email, Phone = person.Phone, personId = person.Id });
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter a valid email address or SSA PIN");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Unable to retreive User");
            }
        }

            public bool IsAnEmailAddress(string emailAddress)
            {
               try 
               { 
                 MailAddress m = new MailAddress(emailAddress); 
                  return true; 
                } 
               catch (Exception ex) 
              { 
                  return false; 
              } 
           }


        // POST: api/PeopleAPI
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PeopleAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PeopleAPI/5
        public void Delete(int id)
        {
        }
    }
}
