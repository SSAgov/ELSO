using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELSO.Data.Repositories
{
    public interface IEventRepository 
    {
        bool SignIn(int eventId, int personId);
    }
}
