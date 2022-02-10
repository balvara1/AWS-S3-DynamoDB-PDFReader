using _301128209alvarado_Lab2.Utility;
using _301128209alvarado_Lab2.Views;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _301128209alvarado_Lab2.ViewModels
{
    public class ViewModelLogin : ViewModelBase
    {
        private string username;
        private string password;

        public string Username
        {
            get
            {
                return username;
            }
            set
            {              
                username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get
            { 
                  return password;
            }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public async void Login()
        {

            QueryResponse res = await DynamoDB.UserLogin(Username, Password.ToString());
            if (res.Count > 0)
            {
                // hide parent window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Hide();

                // pass username data to BooksWindow through data context
                BooksWindow booksWindow = new BooksWindow();
                booksWindow.DataContext = new ViewModelBooks(Username);

                // show subwindow
                booksWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }
    }
}
