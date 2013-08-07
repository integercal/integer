using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ConfigModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Priest { get; set; }
        public string Birth { get; set; } //public DateTime Birth { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string ScheduleStart { get; set; }//public DateTime ScheduleStart { get; set; }
        public string ScheduleEnd { get; set; }//public DateTime ScheduleEnd { get; set; }
        public int ReserveInterval { get; set; }
        public string Version { get; set; }
        public string Since { get; set; }//public DateTime Since { get; set; }
    }
}