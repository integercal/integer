using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string GroupId { get; set; }
        public DateTime DateCreation { get; set; }
        public string Role { get; set; }

        public string FacebookId { get; set; }
        public string Gender { get; set; }
        public string Username { get; set; }
    }
}