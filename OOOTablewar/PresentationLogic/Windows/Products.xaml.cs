using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PresentationLogic.Windows
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>
   
    public partial class Products : Window
    {
        private readonly ProductsViewModel _viewModel;

        public Products()
        {
            InitializeComponent();
            _viewModel = new ProductsViewModel();
            this.DataContext = _viewModel;
        }
    }
}
