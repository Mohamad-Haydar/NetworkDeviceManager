using System.ComponentModel;

namespace DesktopApp.Models
{
    public class LanDevice : INotifyPropertyChanged
    {
        private string _deviceName;
        private string _deviceType;
        private string _ipAddress;
        private string _location;
        private string _status;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string DeviceName
        {
            get { return _deviceName; }
            set
            {
                if (_deviceName != value)
                {
                    _deviceName = value;
                    OnPropertyChanged(nameof(DeviceName)); // Notify UI about the change
                }
            }
        }

        public string DeviceType
        {
            get { return _deviceType; }
            set
            {
                if (_deviceType != value)
                {
                    _deviceType = value;
                    OnPropertyChanged(nameof(DeviceType));
                }
            }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    OnPropertyChanged(nameof(IpAddress));
                }
            }
        }

        public string Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status)); // Crucial for real-time status updates in DataGrid
                }
            }
        }
    }
}
