using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ELSO.Services.ViewModel;
using System.DirectoryServices.AccountManagement;
using System.Net.Http;
using System.Net;
using System.Xml.Linq;
using System.Xml;

namespace ELSO.Services
{
    public class UserService : IUserService
    {
        public bool GetADUserByPIN(string PIN)
        {
            UserViewModel userVM = null;
            try {
                // set up domain context
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain))
                {
                    // find user by display name
                    UserPrincipal ADUser = UserPrincipal.FindByIdentity(ctx, PIN);

                    // 
                    if (ADUser != null)
                    {
                        userVM = new UserViewModel
                        {
                            DisplayName = ADUser.DisplayName,
                            LastName = ADUser.Surname,
                            FirstName = ADUser.GivenName,
                            PIN = ADUser.SamAccountName,
                            Email = ADUser.EmailAddress
                        };
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
            return false;
        }

        public UserViewModel GetGALUserByPIN(string PIN)
        {
            var GALURL = ConfigurationManager.AppSettings.Get("GALURI");
            var consumerId = ConfigurationManager.AppSettings["GALConsumerId"];
            var contextId = ConfigurationManager.AppSettings["GALContextId"];
            UserViewModel user = new UserViewModel();
            var dataPowerHeaders = new WebHeaderCollection           
            {
                "ConsumerID: " + consumerId,
                "ContextID: " + contextId
            };
            var url = $"{GALURL}?REQTYPE=GETALL&PIN={PIN}&Application={consumerId}";
           

            var httpReq = WebRequest.Create(url);
            if (httpReq == null) throw new Exception("Error Requesting Core Services (Data Power)");
            httpReq.Credentials = CredentialCache.DefaultNetworkCredentials;
            httpReq.ContentType = "text/xml;charset=\"utf-8\"";
            httpReq.Method = "GET";
            httpReq.ContentLength = 0;
            httpReq.Headers.Add(dataPowerHeaders);
            var xmlReader = httpReq.GetResponse().GetResponseStream();

            var xDoc = XDocument.Load(xmlReader);


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

                                    // FirstName = FirstName.Substring(0, FirstName.Length - 10);

                                    //var index = displayName[1].Trim().IndexOf("Contractor");
                                    // this.FirstName = displayName[1].Trim().Remove(index); 
                                    user.FirstName= displayName[1].Trim();
                                break;
                            case "pin":
                               user.PIN = xe.Value;
                                break;
                            case "email":
                                user.Email = xe.Value;
                                break;
                            case "office":
                                user.Office = xe.Value;
                                break;
                            case "department":
                                user.Department = xe.Value;
                                break;
                            case "company":
                                user.Company = xe.Value;
                                break;
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
        
        public bool isAuthenticated(string PIN, string domain)
        {
            try {
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
            catch(Exception ex)
            {

            }
            return false;
        }
    }
}
