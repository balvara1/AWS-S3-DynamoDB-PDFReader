using _301128209alvarado_Lab2.Models;
using _301128209alvarado_Lab2.ViewModels;
using _301128209alvarado_Lab2.Utility;
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

namespace _301128209alvarado_Lab2.Views
{
    /// <summary>
    /// Interaction logic for BooksWindow.xaml
    /// </summary>
    public partial class BooksWindow : Window
    {

        public BooksWindow()
        {
            InitializeComponent();
            // set date for the Books Window display
            label_dt.Content = DateTime.Now.ToString("dddd , MMM dd yyyy - hh:mm:ss");
        }

        private void listView_Books_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BookItem bookItem = listView_Books.SelectedItem as BookItem;
            (DataContext as ViewModelBooks).OpenPDFWindow(bookItem);
        }

    }
}
