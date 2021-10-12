using System;
using System.Collections.Generic;
using System.Text;

namespace ASC.Models.Configuration
{
    public class ApplicationSettings
    {
        public string ApplicationTitle { get; set; }
        public string AdminEmail { get; set; }
        public string AdminName { get; set; }
        public string AdminPassWord { get; set; }
        public string Roles { get; set; }
    }
}
