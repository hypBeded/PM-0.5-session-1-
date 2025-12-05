using DataBaseLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLogic.Interfaces
{
    public interface INavigationService
    {
        void NavigateToSignUp(Window currentWindow);
        void NavigateToAddEditProduct(User user);
    }
}
