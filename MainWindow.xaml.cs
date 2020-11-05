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

            // вход без проверки
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-064PORC\SQLEXPRESS;Initial Catalog=CryptoWallets;User ID=root;Password=root");
            bool success = false;
            try
            {
                const string command = "Select * From Users WHERE Login=@UserName AND Password=@Password";
                SqlCommand check = new SqlCommand(command,con);
                check.Parameters.AddWithValue("@UserName", UserName.Text);
                check.Parameters.AddWithValue("@Password", Password.Password);
                con.Open();

                using (var DataReader = check.ExecuteReader())
                {
                    success = DataReader.Read();
                }
            }
            finally
            {
                con.Close();
            }

            if (success)
            {
                Window1 win1 = new Window1();
                win1.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            //хэширование логина
            byte[] bytes = Encoding.UTF8.GetBytes(UserName.Text);
            SHA256Managed sHA256hash = new SHA256Managed();

            byte[] hash = sHA256hash.ComputeHash(bytes);
            string hashstring = string.Empty;
            foreach (byte x in bytes)
            {
                hashstring += String.Format("{0:x2}", x);
            }
            //хэширование пароля
            byte[] Bytes = Encoding.UTF8.GetBytes(Password.Password);
            SHA256Managed shA256hash = new SHA256Managed();

            byte[] Hash = sHA256hash.ComputeHash(Bytes);
            string Hashstring = string.Empty;
            foreach (byte x in Bytes)
            {
                Hashstring += String.Format("{0:x2}", x);
            }
            Proverka1.Text = hashstring;
            Proverka2.Text = Hashstring;
            // регистрация без проверки
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-064PORC\SQLEXPRESS;Initial Catalog=CryptoWallets;User ID=root;Password=root");
            con.Open();
            if (UserName.Text != null && Password.Password != null)
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Users] (Login,Password) VALUES (@UserName,@Password) ", con);
                command.Parameters.AddWithValue("UserName", hashstring);
                command.Parameters.AddWithValue("Password", Hashstring);
                await command.ExecuteNonQueryAsync();
            }
            else
            {
                MessageBox.Show("Данные не введены");
                
            }
            con.Close();
        }
    }
}
