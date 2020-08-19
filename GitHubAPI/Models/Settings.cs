using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubAPI.Models
{
    public class Settings
    {
        public int maxusers { get; set; }
        public int cacheexpirationminutes { get; set; }        
    }
}
