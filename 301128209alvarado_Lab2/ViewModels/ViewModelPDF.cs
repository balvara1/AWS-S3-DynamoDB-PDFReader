using _301128209alvarado_Lab2.Utility;
using _301128209alvarado_Lab2.Models;
using _301128209alvarado_Lab2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Amazon.S3.Model;
using System.Windows;

namespace _301128209alvarado_Lab2.ViewModels
{
    public class ViewModelPDF : ViewModelBase
    {
        private readonly PDFWindow pDFWindow;
        private bool isBookmarked = false;
        private BookItem book;
        private MemoryStream memoryStream;

        public bool IsBookmarked
        {
            get { return isBookmarked; }
            set
            {
                isBookmarked = value;
                OnPropertyChanged();
            }
        }

        public MemoryStream Document
        {
            get { return memoryStream; }
            set
            {
                memoryStream = value;
                OnPropertyChanged();
            }
        }

        public ViewModelPDF(PDFWindow pdfWindow, BookItem bookItem)
        {
            pDFWindow = pdfWindow;
            book = bookItem;
            IsBookmarked = bookItem.IsBookmarked;
            OpenBook(bookItem.ISBN);         
        }

        private async void OpenBook(string isbn)
        {
            // isbn should match the key for S3 bucket
            GetObjectResponse res = await AWSS3.LoadBook(isbn);
            MemoryStream stream = new MemoryStream();
            res.ResponseStream.CopyTo(stream);
            Document = stream;
            if (IsBookmarked)
                pDFWindow.PDFViewer.GoToPageAtIndex(book.Page);
        }

        public async void CloseBook(int markedpage, long timeLastViewed)
        {
           // set IsBookmarked to true and page to markedpage
           // so book will resume automatically reopen on that page
            book.IsBookmarked = true;
            book.Page = markedpage;
            book.TimeLastViewed = timeLastViewed;

            /*book.IsBookmarked = IsBookmarked;
            if (isBookmarked)
            {
                book.Page = markedpage;
                book.TimeLastViewed = timeLastViewed;
                //MessageBox.Show(timeLastViewed.ToString());
            }
            else
            {
                book.Page = 1;
                book.TimeLastViewed = timeLastViewed;
                //MessageBox.Show(timeLastViewed.ToString());
            }*/
            await DynamoDB.UpdateBook(book);
        }
        
        
    }
}
