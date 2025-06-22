using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp.Helper
{
    public class PingHelper
    {
        public static async Task<string> GetStatusAsync(string ip)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(ip, 1000);
                return reply.Status == IPStatus.Success ? "Online" : "Offline";
            }
            catch
            {
                return "Offline";
            }
        }
    }
}
