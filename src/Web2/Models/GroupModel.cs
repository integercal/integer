using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class GroupModel
    {
        public GroupModel()
        {
            Children = new List<GroupModel>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ParentId { get; set; }
        public string Color { get; set; }
        public IList<GroupModel> Children { get; set; }
    }
}