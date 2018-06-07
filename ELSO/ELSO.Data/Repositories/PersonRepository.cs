using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELSO.Model;
using System.Configuration;
using System.Net;
using System.Xml.Linq;
using System.Xml;

namespace ELSO.Data.Repositories
{
    public class PersonRepository : Repository<Person>
    {
        public PersonRepository(dELSO3AEntities context)
            : base(context)
        {
        }
        public Person GetFromGAL(string Pin, string Email = null)
        {
            
            var GALURL = ConfigurationManager.AppSettings.Get("GALURI");
            var consumerId = ConfigurationManager.AppSettings["GALConsumerId"];
            var contextId = ConfigurationManager.AppSettings["GALContextId"];
            var url = "";
            Person user = new Person();
            var dataPowerHeaders = new WebHeaderCollection
            {
                "ConsumerID: " + consumerId,
                "ContextID: " + contextId
            };
            if (String.IsNullOrEmpty(Pin) == false)
            {
                url = $"{GALURL}?REQTYPE=GETALL&PIN={Pin}&Application={consumerId}";
            }
            if (String.IsNullOrEmpty(Email) == false)
            {
                url = $"{GALURL}?REQTYPE=GETALL&EMAIL={Email}&Application={consumerId}";
            }
            var httpReq = WebRequest.Create(url);
            if (httpReq == null) throw new Exception("Error Requesting Core Services (Data Power)");
            httpReq.Credentials = CredentialCache.DefaultNetworkCredentials;
            httpReq.ContentType = "text/xml;charset=\"utf-8\"";
            httpReq.Method = "GET";
            httpReq.ContentLength = 0;
            httpReq.Headers.Add(dataPowerHeaders);
            var xmlReader = httpReq.GetResponse().GetResponseStream();

            var xDoc = System.Xml.Linq.XDocument.Load(xmlReader);


            foreach (XElement xe in xDoc.Descendants())
            {
                switch (xe.NodeType)
                {
                    case XmlNodeType.Element:
                        var insideNode = xe.Name.ToString();
                        switch (insideNode)
                        {
                            case "displayName":
                                string[] displayName = xe.Value.ToString().Split(',');
                                user.LastName = displayName[0].Trim();
                                if (displayName.Length > 1)
                                {
                                    user.FirstName = displayName[1].Trim();
                                }
                                break;
                            case "pin":
                                user.SSA_PIN = xe.Value;
                                break;
                            case "email":
                                user.Email = xe.Value;
                                break;
                            case "company":
                                user.Organization = xe.Value;
                                break;
                            case "telephone":
                                user.Phone = xe.Value;
                                break;
                            case "department":
                                break;
                            //case "company":
                            //    //user. = xe.Value;
                            //    break;
                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }

            }
            return user;
        }
    }
}
