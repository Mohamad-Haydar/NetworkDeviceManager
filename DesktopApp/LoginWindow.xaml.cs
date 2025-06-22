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
            if (UserName.Text == "admin" && Password.Text == "1234")
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
