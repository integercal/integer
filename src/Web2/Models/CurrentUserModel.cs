using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web2.Models
{
    public class CurrentUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GroupId { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        
        public string PictureUrl 
        { 
            get 
            { 
                string url = "";
                if (!String.IsNullOrWhiteSpace(Username))
                    url = String.Format("//graph.facebook.com/{0}/picture", Username);

                return url;
            } 
        }
    }
}