using DataBaseLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLogic.Interfaces
{
    public interface IDatabaseService
    {
        User AuthenticateUser(string email, string password);
    }
}
