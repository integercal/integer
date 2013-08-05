using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class Evento
    {
        public string Id { get; set; }

        public string Subject { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }
    }
}