using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Project_Balakin
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();

        }
        private void Button_Click(object sender, RoutedEventArgs e)

        {
            if (box_login.Text.Length > 0)
            {
                if (box_password.Password.Length > 0)
                {
                    using (var DataBase = new mainEntities())
                    {
                        string login = box_login.Text;
                        string password = box_password.Password;

                        var isUserExistsLoginAdm = DataBase.admins.Any(u => u.login == login);
                        var isUserExistsPassAdm = DataBase.admins.Any(u => u.password == password);
                        var isUserExistsLogin = DataBase.users.Any(u => u.login == login);
                        var isUserExistsPass = DataBase.users.Any(u => u.password == password);

                        if (isUserExistsLoginAdm && isUserExistsPassAdm)
                        {
                            MessageBox.Show("Админ авторизовался");
                            ClassChangePage.frame1.Navigate(new AdminPage());
                        }
                        else
                        {
                            if (isUserExistsLogin && isUserExistsPass)
                            {
                                MessageBox.Show("Пользователь авторизовался");
                                ClassChangePage.frame1.Navigate(new Main());
                            }
                            else MessageBox.Show("Неверный логин или пароль");
                        }
                    }
                }
                else MessageBox.Show("Введите пароль");
            }
            else MessageBox.Show("Введите логин");
        }

        private void click_Reg(object sender, RoutedEventArgs e)
        {
            ClassChangePage.frame1.Navigate(new Registration());
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (box_password.Password.Length > 0)
            {
                Watermark.Visibility = Visibility.Collapsed;
            }
            else
            {
                Watermark.Visibility = Visibility.Visible;
            }
        }
    }
}
