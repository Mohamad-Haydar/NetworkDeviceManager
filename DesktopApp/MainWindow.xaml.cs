using DesktopApp.Helper;
using DesktopApp.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace DesktopApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Device> Devices = [];
        private ICollectionView DevicesView;
        private DispatcherTimer _statusTimer;
        public MainWindow()
        {
            InitializeComponent();
            LoadDevices();

            DevicesView = CollectionViewSource.GetDefaultView(Devices);
            deviceDataGrid.ItemsSource = DevicesView;

            _statusTimer = new DispatcherTimer();
            _statusTimer.Interval = TimeSpan.FromSeconds(1); // every 2 seconds
            _statusTimer.Tick += UpdateStatuses;
            _statusTimer.Start();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddDeviceWindow();
            bool? result = window.ShowDialog();

            if (result == true)
            {
                LoadDevices(); // Re-fetch updated list from DB
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
        }

        

       

        // Update the status of the devices every 2 seconds
        private async void UpdateStatuses(object sender, EventArgs e)
        {
            var deviceSnapshot = Devices.ToList();
            foreach (var device in deviceSnapshot)
            {
                string status = await PingHelper.GetStatusAsync(device.IpAddress);
                device.Status = status;
            }
        }

        private void ScanLan_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new DiscoverDevicesWindow();
            mainWindow.Show();
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            DevicesView.Filter = FilterByIp;
            DevicesView.Refresh();
        }

        private bool FilterByIp(object obj)
        {
            if (obj is Device device)
            {
                string? filter = txtFilter.Text?.Trim();
                if (string.IsNullOrEmpty(filter))
                    return true;

                return device.IpAddress != null && device.IpAddress.Contains(filter, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Device selectedDevice)
            {
                var res = MessageBox.Show($"Editing device: {selectedDevice.DeviceName}");
                if(res == MessageBoxResult.OK)
                {
                    var window = new EditDeviceWindow(selectedDevice);
                    bool? result = window.ShowDialog();

                    if (result == true)
                    {
                        LoadDevices(); // Re-fetch updated list from DB
                    }
                }
                
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Device selectedDevice)
            {
                var result = MessageBox.Show($"Are you sure you want to delete '{selectedDevice.DeviceName}'?",
                                             "Confirm Delete",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    DeviceService.DeleteDevice(selectedDevice);
                    Devices.Remove(selectedDevice);
                    LoadDevices(); // Refresh table
                }
            }
        }
    }
}