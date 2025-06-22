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

        public static void DeleteDevice(Device device)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();

            string query = "DELETE FROM devices WHERE id = @id";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", device.Id);
            cmd.ExecuteNonQuery();
        }
    }
}
