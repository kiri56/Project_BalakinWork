﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Project_Balakin
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();

            BrushConverter converter = new BrushConverter();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 1), DispatcherPriority.Normal, delegate
            {
                
            if (GlobalVar.ThemeChanged == true)
                {
                
                label1.Foreground = (Brush)converter.ConvertFromString("White");
                label2.Foreground = (Brush)converter.ConvertFromString("White");
                label3.Foreground = (Brush)converter.ConvertFromString("White");
                }

            if (GlobalVar.ThemeChanged == false)
                {
                
                label1.Foreground = (Brush)converter.ConvertFromString("Black");
                label2.Foreground = (Brush)converter.ConvertFromString("Black");
                label3.Foreground = (Brush)converter.ConvertFromString("Black");
                }
            }, Dispatcher);
        }
        private void Check_click(object sender, RoutedEventArgs e)
        {
            if (box_login.Text.Length > 0) 
            {
                if (box_password.Password.Length > 0) 
                {
                    if (box_password.Password.Length >= 6)
                    {
                        bool en = true; // английская раскладка
                        bool number = false; 

                        for (int i = 0; i < box_password.Password.Length; i++) 
                        {
                            if (box_password.Password[i] >= 'А' && box_password.Password[i] <= 'Я') en = false; // если русская раскладка
                            if (box_password.Password[i] >= '0' && box_password.Password[i] <= '9') number = true; // если цифры
                        }

                        if (!en)
                            MessageBox.Show("Доступна только английская раскладка"); 
                        else if (!number)
                            MessageBox.Show("Добавьте хотя бы одну цифру"); 
                        if (en && number) 
                        {
                            if (box_check_password.Password.Length > 0) 
                            {
                                if (box_password.Password == box_check_password.Password) 
                                {
                                    string login = box_login.Text;
                                    string password = box_password.Password;
                                    int id = 1;

                                    using (var DataBase = new mainEntities())
                                    {

                                        var isUserExists = DataBase.users.Any(u => u.login == login);

                                        if (isUserExists)
                                        {
                                            MessageBox.Show("Такой пользователь уже существует");
                                        }
                                        else
                                        {
                                            InsertUser(login, password, id);
                                            MessageBox.Show("Пользователь зарегистрирован");
                                            ClassChangePage.frame1.Navigate(new Login());
                                        }
                                    }
                                }
                                else MessageBox.Show("Пароли не совпадают");
                            }
                            else MessageBox.Show("Повторите пароль");
                        }
                    }
                    else MessageBox.Show("Пароль слишком короткий, минимум 6 символов");
                }
                else MessageBox.Show("Укажите пароль");
            }
            else MessageBox.Show("Укажите логин");
        }
        private static bool InsertUser(string login, string password, int id)
        {
            bool result = false;

            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-MJUKOVB\\SQLEXPRESS;Database=main;Trusted_Connection=True;MultipleActiveResultSets=True;"))
            {
                string query = "INSERT INTO users (login, password, id) VALUES (@login, @password, @id)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    try
                    {
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@id", id);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            result = true;
                        }
                        else
                        {
                            MessageBox.Show("Error: User not inserted");
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            return result;
        }

        private void Back_click(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new Login());
        }

        private void OnPasswordChanged1(object sender, RoutedEventArgs e)
        {
            if (box_password.Password.Length > 0)
            {
                Watermark2.Visibility = Visibility.Collapsed;
            }
            else
            {
                Watermark2.Visibility = Visibility.Visible;
            }
        }
        private void OnPasswordChanged2(object sender, RoutedEventArgs e)
        {
            if (box_check_password.Password.Length > 0)
            {
                Watermark1.Visibility = Visibility.Collapsed;
            }
            else
            {
                Watermark1.Visibility = Visibility.Visible;
            }
        }
    }
}

