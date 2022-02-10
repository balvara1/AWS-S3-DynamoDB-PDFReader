using _301128209alvarado_Lab2.Views;
using _301128209alvarado_Lab2.Utility;
using _301128209alvarado_Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace _301128209alvarado_Lab2.ViewModels
{
    public class ViewModelBooks : ViewModelBase
    {
        
        public string username;
       
        private string _displayName = "default_username";     
        private ObservableCollection<BookItem> _books = new ObservableCollection<BookItem>();
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookItem> Books
        {
            get
            {
                return _books;
            }
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        public ViewModelBooks(String currentUser)
        {
            // set DisplayName to currentUser input
            DisplayName = currentUser;

            // load books 
            LoadUserBooks(currentUser);
        }

        private async void LoadUserBooks(string username)
        {
            
            List<BookItem> books = await DynamoDB.GetBooks(username);

            // sort books according to timeLastViewed (most recently opened)
            List<BookItem> sortBookList = books.OrderByDescending(book => book.TimeLastViewed).ToList();
            foreach (BookItem book in sortBookList)
            {
                Books.Add(book);
            }
        }

        public void OpenPDFWindow(BookItem book)
        {
            PDFWindow pDFWindow= new PDFWindow(book);
            pDFWindow.Show();
        }
    }
}
