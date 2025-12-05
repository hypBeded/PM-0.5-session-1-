using DataBaseLogic;
using PresentationLogic.Interfaces;
using PresentationLogic.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLogic.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateToSignUp(Window currentWindow)
        {
            var signUp = new SignUp();
            signUp.Show();
            currentWindow.Close();
        }

        public void NavigateToProducts(User user)
        {
            var products = new Products(user);
            products.Show();
        }

        public void NavigateToAddEditProduct(User user)
        {
            var addEditProduct = new AddEditProbuct(user);
            addEditProduct.Show();
        }
    }
}
