using _301128209alvarado_Lab2.Models;
using _301128209alvarado_Lab2.ViewModels;
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
    /// Interaction logic for PDFWindow.xaml
    /// </summary>
    public partial class PDFWindow : Window
    {
        //private BookItem book;
        public PDFWindow(BookItem thisbook)
        {
            InitializeComponent();
            DataContext = new ViewModelPDF(this, thisbook);
        }

        private void ClosedWindow(object sender, EventArgs e)
        {
            long timeLastViewed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            

            // send bookmarked page to the PDF ViewModel
            int page = PDFViewer.CurrentPageIndex;
            (DataContext as ViewModelPDF).CloseBook(page, timeLastViewed);

            //test date/time
            //MessageBox.Show(unixTime.ToString());
            
        }
    }
}
