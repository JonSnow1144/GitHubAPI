using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubAPI.Models
{
    public class Users
    {
        public string User { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Company { get; set; }
        public int Followers { get; set; }
        public int Public_Repos { get; set; }
    }
}
