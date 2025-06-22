using DesktopApp.Helper;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace DesktopApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Device> Devices = [];
        private DispatcherTimer _statusTimer;
        public MainWindow()
        {
            InitializeComponent();
            LoadDevices();

            _statusTimer = new DispatcherTimer();
            _statusTimer.Interval = TimeSpan.FromSeconds(2); // every 2 seconds
            _statusTimer.Tick += UpdateStatuses;
            _statusTimer.Start();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Show AddDeviceWindow
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Open EditDialog with selected device
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selected = (Device)deviceDataGrid.SelectedItem;
            if (selected != null)
            {
                DeviceService.DeleteDevice(selected);
                Devices.Remove(selected);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }

        private void LoadDevices()
        {
            Devices.Clear();
            var loaded = DeviceService.LoadDevices(); // from SQLite
            foreach (var d in loaded)
            {
                d.Status = "Checking...";
                Devices.Add(d);
            }
            deviceDataGrid.ItemsSource = Devices;
        }

        // Update the status of the devices every 2 seconds
        private async void UpdateStatuses(object sender, EventArgs e)
        {
            foreach (var device in Devices)
            {
                string status = await PingHelper.GetStatusAsync(device.IpAddress);
                device.Status = status;
            }
        }
    }
}