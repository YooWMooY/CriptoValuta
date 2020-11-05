using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CriptoValuta.Classes
{
    class Login
    {
        public Login()
        {

        }


        public Boolean Authoriz(string login, string password)
        {

            // вход без проверки
            SqlConnection con = new SqlConnection(@"Data Source=ALEXANDER\SQLEXPRESS;Initial Catalog=CryptoWallets;User ID=root;Password=root");
            bool success = false;
            try
            {
                const string command = "Select * From Users WHERE Login=@UserName AND Password=@Password";
                SqlCommand check = new SqlCommand(command, con);
                check.Parameters.AddWithValue("@UserName", login);
                check.Parameters.AddWithValue("@Password", password);
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
                Window1 win1 = new Window1(login);
                win1.Show();
                return true;
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
                return false;
            }

        }

        public async Task<bool> RegisterAsync(string login, string password)
        {
            //хэширование логина
            byte[] bytes = Encoding.UTF8.GetBytes(login);
            SHA256Managed sHA256hash = new SHA256Managed();

            byte[] hash = sHA256hash.ComputeHash(bytes);
            string hashstring = string.Empty;
            foreach (byte x in bytes)
            {
                hashstring += String.Format("{0:x2}", x);
            }
            //хэширование пароля
            byte[] Bytes = Encoding.UTF8.GetBytes(password);
            SHA256Managed shA256hash = new SHA256Managed();

            byte[] Hash = sHA256hash.ComputeHash(Bytes);
            string Hashstring = string.Empty;
            foreach (byte x in Bytes)
            {
                Hashstring += String.Format("{0:x2}", x);
            }

            // регистрация без проверки
            SqlConnection con = new SqlConnection(@"Data Source=ALEXANDER\SQLEXPRESS;Initial Catalog=CryptoWallets;User ID=root;Password=root");
            con.Open();
            if (login != "" && password != "")
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Users] (Login,Password) VALUES (@UserName,@Password) ", con);
                command.Parameters.AddWithValue("UserName", hashstring);
                command.Parameters.AddWithValue("Password", Hashstring);
                await command.ExecuteNonQueryAsync();
                con.Close();
                return true;
            }
            else
            {
                MessageBox.Show("Данные не введены");
                con.Close();
                return false;

            }
        }

    }
}
