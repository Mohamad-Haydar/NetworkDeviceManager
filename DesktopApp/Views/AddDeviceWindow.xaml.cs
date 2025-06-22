using System.Windows;
using System.Xml.Linq;

namespace DesktopApp.Views
{
    public partial class AddDeviceWindow : Window
    {
        public AddDeviceWindow()
        {
            InitializeComponent();
        }

        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            if (DeviceName == null || DeviceType == null || IpAddress == null || Location == null)
            {
                MessageBox.Show("You Should Fill all Input");
            }
            string name = DeviceName.Text.Trim();
            string type = DeviceType.Text.Trim();
            string ip = IpAddress.Text.Trim();
            string location = Location.Text.Trim();

            bool success = DeviceService.AddDevice(name, type, ip, location);

            this.DialogResult = true;  // <== tells the parent window it was successful
            this.Close();
        }
    }
}
