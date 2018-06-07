using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELSO.Services.ViewModel;

namespace ELSO.Services
{
    interface IUserService
    {
        bool isAuthenticated(string PIN, string domain);
        Boolean GetADUserByPIN(string pin);
        UserViewModel GetGALUserByPIN(string pin);
    }
}
