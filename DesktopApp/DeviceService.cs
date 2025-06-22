using Microsoft.Data.Sqlite;

namespace DesktopApp
{
    public class DeviceService
    {
        private const string ConnectionString = "Data Source=DataFile.db";

        public static List<Device> LoadDevices()
        {
           
            var devices = new List<Device>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            string query = "SELECT * FROM Devices";
            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                devices.Add(new Device
                {
                    Id = reader.GetInt32(0),
                    DeviceName = reader.GetString(1),
                    DeviceType = reader.GetString(2),
                    IpAddress = reader.GetString(3),
                    Location = reader.GetString(4),
                });
            }

            return devices;
        }

        public static bool AddDevice(string name, string type, string ip, string location)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();

            string query = "INSERT INTO Devices (DeviceName, DeviceType, IpAddress, Location) VALUES (@name, @type, @ip, @location)";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@ip", ip);
            cmd.Parameters.AddWithValue("@location", location);

            int rowsAffected = cmd.ExecuteNonQuery();
            return rowsAffected > 0;

        }


        public static bool DeleteDevice(Device device)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();

            string query = "DELETE FROM Devices WHERE id = @id";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", device.Id);
            cmd.ExecuteNonQuery();

            return true;
        }

        public static void UpdateDevice(Device device)
        {
            using var conn = new SqliteConnection("Data Source=DataFile.db");
            conn.Open();

            string query = @"UPDATE Devices 
                     SET DeviceName = @name, 
                         DeviceType = @type, 
                         IpAddress = @ip, 
                         Location = @location 
                     WHERE Id = @id";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", device.DeviceName);
            cmd.Parameters.AddWithValue("@type", device.DeviceType);
            cmd.Parameters.AddWithValue("@ip", device.IpAddress);
            cmd.Parameters.AddWithValue("@location", device.Location);
            cmd.Parameters.AddWithValue("@id", device.Id);

            cmd.ExecuteNonQuery();
        }
    }
}
