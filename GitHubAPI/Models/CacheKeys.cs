using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitHubAPI.Models
{
    public class CacheKeys
    {
        public string User { get { return "_User"; } }
        public string Name { get { return "_Name"; } }
        public string Login { get { return "_Login"; } }
        public string Company { get { return "_Company"; } }
        public string Followers { get { return "_Followers"; } }
        public string Public_Repos { get { return "_Public_Repos"; } }
    }
}
