using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Agenda;

namespace Web.Models
{
    public class EventTypeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
    }
}