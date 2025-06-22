using DesktopApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Data;
using System.Windows.Threading;
using DesktopApp.Helper;

namespace DesktopApp
{
    public partial class DiscoverDevicesWindow : Window
    {
        public ObservableCollection<LanDevice> LanDevices { get; set; }
        private DispatcherTimer _statusTimer;
        public DiscoverDevicesWindow()
        {
            InitializeComponent();
            

            LanDevices = new ObservableCollection<LanDevice>();
            this.DataContext = this;

            _ = LoadDevices();

            deviceDataGrid.ItemsSource = LanDevices;
        }



        private async Task LoadDevices()
        {
            try
            {
                #region Get Ip and netmask
                // Get local IP address and subnet mask
                IPAddress localIp = null;
                IPAddress subnetMask = null;

                // Iterate through network interfaces to find a suitable IPv4 address
                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // Consider only operational Ethernet or Wi-Fi adapters
                    if (adapter.OperationalStatus == OperationalStatus.Up &&
                        (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                         adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                    {
                        IPInterfaceProperties ipProps = adapter.GetIPProperties();
                        foreach (UnicastIPAddressInformation ip in ipProps.UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) // IPv4
                            {
                                localIp = ip.Address;
                                subnetMask = ip.IPv4Mask;
                                break; // Found an IPv4 address, break inner loop
                            }
                        }
                    }
                    if (localIp != null)
                    {
                        break; // Found a local IP, break outer loop
                    }
                }

                if (localIp == null || subnetMask == null)
                {
                    Dispatcher.Invoke(() => MessageBox.Show("Could not determine local IP address or subnet mask. Please check your network configuration."));
                    return;
                }
                #endregion

                // Calculate network and broadcast addresses
                byte[] ipBytes = localIp.GetAddressBytes();
                byte[] maskBytes = subnetMask.GetAddressBytes();
                byte[] networkAddressBytes = new byte[4];
                byte[] broadcastAddressBytes = new byte[4];

                for (int i = 0; i < 4; i++)
                {
                    networkAddressBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                    broadcastAddressBytes[i] = (byte)(networkAddressBytes[i] | (~maskBytes[i] & 0xFF));
                }

                IPAddress networkAddress = new IPAddress(networkAddressBytes);
                IPAddress broadcastAddress = new IPAddress(broadcastAddressBytes);

                uint startIp = BitConverter.ToUInt32(networkAddress.GetAddressBytes().Reverse().ToArray(), 0) + 1;
                uint endIp = BitConverter.ToUInt32(broadcastAddress.GetAddressBytes().Reverse().ToArray(), 0) - 1;

                var ipRange = Enumerable.Range((int)startIp, (int)(endIp - startIp + 1))
                                .Select(i => new IPAddress(BitConverter.GetBytes(i).Reverse().ToArray()))
                                .ToList();

                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Starting network scan. This may take a moment...", "Scan Initiated", MessageBoxButton.OK, MessageBoxImage.Information);
                    LanDevices.Clear();
                });

                int maxConcurrency = 50;
                var throttler = new SemaphoreSlim(maxConcurrency);

                var tasks = ipRange.Select(async ip =>
                {
                    await throttler.WaitAsync();
                    try
                    {
                        if (ip.Equals(localIp))
                        {
                            AddOnlineDevice(ip.ToString(), "Local Machine", "This PC");
                            return;
                        }

                        using (Ping pingSender = new Ping())
                        {
                            PingReply reply = await pingSender.SendPingAsync(ip, 1000);
                            if (reply.Status == IPStatus.Success)
                            {
                                AddOnlineDevice(ip.ToString(), "Unknown", "Active Host");
                            }
                        }
                    }
                    catch { /* Ignore offline hosts */ }
                    finally
                    {
                        throttler.Release();
                    }
                }).ToList();

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => MessageBox.Show($"An error occurred during network discovery: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
            }
        }


        private void AddOnlineDevice(string ipAddress, string deviceName, string deviceType)
        {
            Dispatcher.Invoke(() =>
            {
                if (!LanDevices.Any(d => d.IpAddress == ipAddress))
                {
                    LanDevices.Add(new LanDevice
                    {
                        IpAddress = ipAddress,
                        DeviceName = deviceName,
                        DeviceType = deviceType,
                        Location = "LAN",
                        Status = "Online"
                    });
                }
            });
        }
    }
}
