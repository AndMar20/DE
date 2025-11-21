using System.Windows;
using System.Windows.Controls;

namespace DE.Client.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly Frame _mainFrame;

        public LoginPage(Frame mainFrame)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
        }   

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                _mainFrame.Navigate(new ProductsPage());
            }

            else
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ProductsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new ProductsPage());
        }
    }
}
