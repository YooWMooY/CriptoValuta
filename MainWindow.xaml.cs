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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Security.Cryptography;
using CriptoValuta.Classes;

namespace CriptoValuta
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            
            if (login.Authoriz(UserName.Text, Password.Password))
            {
                this.Close();
            }
            
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            Login register = new Login();

            if (await register.RegisterAsync(UserName.Text, Password.Password))
            {
                MessageBox.Show("Вы успешнно зарегистрировались!");
            }
        }
    }
}
