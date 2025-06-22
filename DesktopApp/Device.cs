using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopApp
{
    public class Device
    {
        [Key]
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
    }
}
