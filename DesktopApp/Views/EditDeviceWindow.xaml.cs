using System.Windows;

namespace DesktopApp.Views
{
    public partial class EditDeviceWindow : Window
    {
        private readonly Device _device;
        public EditDeviceWindow(Device device)
        {
            InitializeComponent();

            _device = device;

            // Pre-fill values
            DeviceName.Text = device.DeviceName;
            DeviceType.Text = device.DeviceType;
            IpAddress.Text = device.IpAddress;
            Location.Text = device.Location;

            AddDevice.Click += Submit_Click;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            // Update values from textboxes
            _device.DeviceName = DeviceName.Text;
            _device.DeviceType = DeviceType.Text;
            _device.IpAddress = IpAddress.Text;
            _device.Location = Location.Text;

            // Save to DB
            DeviceService.UpdateDevice(_device);

            this.DialogResult = true;
            this.Close();
        }
    }
}
