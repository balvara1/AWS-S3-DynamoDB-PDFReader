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
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;

namespace _301128209alvarado_Lab2.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // bind data 
            DataContext = new ViewModelLogin();
            InitUser();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            string un = textBox_username.Text;
            string pw = passwordBox_password.Password;

            // null validation
            try
            {
                if (string.IsNullOrEmpty(un) && string.IsNullOrEmpty(pw))
                {
                   MessageBox.Show("Username and Password cannot be empty", "Error");
                   this.Close();
                }
                else
                {
                    (DataContext as ViewModelLogin).Login();
                    // clear password box
                    passwordBox_password.Password = "";
                }
            }
            catch (AmazonDynamoDBException exception)
            {
                 Console.WriteLine("An error occurred on the server side " + exception.Message);
            }

        }

        private async void InitUser()
        {
            try
            {
                ListTablesResponse listTableResponse = await DynamoDB.ListTables();
                DynamoDB.CreateTable();
            }
            catch (AmazonDynamoDBException exception)
            {
                Console.WriteLine("An error occurred on the server side " + exception.Message);
            }
        }

        private void passwordBox_password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModelLogin).Password = (sender as PasswordBox).Password;
        }
    }
}
