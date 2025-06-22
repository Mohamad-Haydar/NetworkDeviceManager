using System.Windows;

namespace DesktopApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserName.Text;
            var password = Password.Text;
            using (DataContext context = new DataContext())
            {
                bool userFound = context.Users.Any(user => user.Name == userName && user.Password == password);
                if (userFound)
                {
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }
        }
    }
}
