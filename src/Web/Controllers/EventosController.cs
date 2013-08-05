using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Models;

namespace Web.Controllers
{
    public class EventosController : ApiController
    {
        public IEnumerable<Evento> Get() 
        {
            return new List<Evento> { 
                new Evento{ 
                    Id= "event1",
                  	Subject= "Green event.",
                  	Start= DateTime.Now,
                  	End= DateTime.Now.AddHours(3),
					Description= "The green event." },
                new Evento{ 
                    Id= "event2",
                  	Subject= "Red event.",
                  	Start= DateTime.Now.AddHours(1),
                  	End= DateTime.Now.AddHours(4),
					Description= "The red event." }
            };
        }
    }
}
