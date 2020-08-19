using GitHubAPI.Models;
using GitHubAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GitHubAPI.Controllers
{
    [Route("api/[controller]")]
    public class GitHubController: Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private Settings _settings;

        public GitHubController(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _cache = memoryCache;
            _configuration = configuration;
            GetSettings();
        }

        [HttpGet("[action]")]
        public List<Users> GitUsers(string[] usernames)
        {
            try
            {
                MemoryCacheEntryOptions cacheExpirationOptions = new MemoryCacheEntryOptions();
                cacheExpirationOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(_settings.cacheexpirationminutes);
                cacheExpirationOptions.Priority = CacheItemPriority.Normal;

                //Array.Sort(usernames);
                List<Users> userList = new List<Users>();
                foreach (string user in usernames) {
                    if (!string.IsNullOrEmpty(user))
                    {
                        //Get cached user first
                        var gituser = GetCachedUser(user);

                        //Not cached, get from GitHub
                        if (string.IsNullOrWhiteSpace(gituser.Name))
                        {
                            var apiUrl = $"https://api.github.com/users/{user}";
                            gituser = JsonConvert.DeserializeObject<Users>(APIHelper.SendAPIRequest(
                               ""
                               , apiUrl
                               , null
                               , "get"
                           ));
                            gituser.User = user;
                            _cache.Set(user, gituser, cacheExpirationOptions);
                        }

                        userList.Add(gituser);

                        if (userList.Count >= _settings.maxusers) break;
                    }
                }
                return userList.OrderBy(a => a.Name).ToList();
            }
            catch 
            {
                return new List<Users>();
            }
        }

        private Users GetCachedUser(string key) {
            Users user = new Users();
            if (_cache.TryGetValue(key, out Users cachedgituser))
            {
                user.User = cachedgituser.User;
                user.Name = cachedgituser.Name;
                user.Login = cachedgituser.Login;
                user.Company = cachedgituser.Company;
                user.Followers = cachedgituser.Followers;
                user.Public_Repos = cachedgituser.Public_Repos;
            }
            return user;
        }

        private void GetSettings()
        {
            var config = new SettingsHelper(_configuration);
            _settings = config.GetSettings();
            //var setting = _configuration.GetSection("GitHubSettings");
            //_settings.maxusers = int.TryParse(setting["url"], out int max) ? max : 10;
            //_settings.cacheexpirationminutes = int.TryParse(setting["cacheexpirationminutes"], out int minutes) ? minutes : 2;
        }
    }
}
