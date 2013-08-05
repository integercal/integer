using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Integer.Domain.Agenda;

namespace Web.Models
{
    public class EventModel
    {
        public string Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public EventTypeModel EventType { get; set; }
        public GroupModel Group { get; set; }
        public IList<LocalModel> Locals { get; set; }
        public string Color { get; set; }
        public IList<RSVPModel> Rsvp { get; set; }
    }
}