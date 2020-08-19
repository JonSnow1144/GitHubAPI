//using Microsoft.AspNetCore.Mvc;
using GitHubAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using static GitHubAPI.Controllers.GitHubController;

namespace APITester
{
    public class GitUserAPITest
    {
        [Fact]
        public async Task GitUsers_TestAsync()
        {
            using (var client = new ClientProvider().Client)
            {
                var response = await client.GetAsync("api/github/gitusers");
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { "jonsnow1144", "technoweenie" } })]
        public async Task GitUsers_TestWithTwoResults(string[] users)
        {
            using (var client = new ClientProvider().Client)
            {
                string param = string.Join("&", users.Select(x => "usernames=" + x));
                var response = await client.GetAsync($"api/github/gitusers/?{ param }");
                var gitusers = JsonConvert.DeserializeObject<List<Users>>(response.Content.ReadAsStringAsync().Result);
                Assert.Equal(2, gitusers.Count);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { "jonsnow1144" } })]
        public async Task GitUsers_TestValidResults(string[] users)
        {
            using (var client = new ClientProvider().Client)
            {
                string param = string.Join("&", users.Select(x => "usernames=" + x));
                var response = await client.GetAsync($"api/github/gitusers/?{ param }");                
                var gitusers = JsonConvert.DeserializeObject<List<Users>>(response.Content.ReadAsStringAsync().Result);
                Assert.Equal("jonsnow1144", gitusers[0].User);
                Assert.Equal("Carl", gitusers[0].Name);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { "invaliduser1234", "invaliduser5678" } })]
        public async Task GitUsers_TestInvalidResults(string[] users)
        {
            using (var client = new ClientProvider().Client)
            {
                string param = string.Join("&", users.Select(x => "usernames=" + x));
                var response = await client.GetAsync($"api/github/gitusers/?{ param }");
                var gitusers = JsonConvert.DeserializeObject<List<Users>>(response.Content.ReadAsStringAsync().Result);

                //invaliduser1234
                Assert.Equal("invaliduser1234", gitusers[0].User);
                Assert.Null(gitusers[0].Name);

                //invaliduser5678
                Assert.Equal("invaliduser5678", gitusers[1].User);
                Assert.Null(gitusers[1].Name);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { } })]
        public async Task GitUsers_TestNoResults(string[] users)
        {
            using (var client = new ClientProvider().Client)
            {
                string param = string.Join("&", users.Select(x => "usernames=" + x));
                var response = await client.GetAsync($"api/github/gitusers");
                var gitusers = JsonConvert.DeserializeObject<List<Users>>(response.Content.ReadAsStringAsync().Result);

                Assert.Empty(gitusers);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { "ivey", "wayneeseguin", "kevinclark", "macournoyer", "caged", "anotherjesse", "fanvsfan", "tomtt", "railsjitsu", "nitay", "kevwil", "jamesgolick" } })]
        public async Task GitUsers_TestMustReturnMaxResultsOnly(string[] users)
        {
            using (var client = new ClientProvider().Client)
            {
                string param = string.Join("&", users.Select(x => "usernames=" + x));
                var response = await client.GetAsync($"api/github/gitusers/?{ param }");
                var gitusers = JsonConvert.DeserializeObject<List<Users>>(response.Content.ReadAsStringAsync().Result);

                Assert.Equal(10, gitusers.Count);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { "jonsnow1144", "", "kevinclark", "" } })]
        public async Task GitUsers_TestEmptyUsers(string[] users)
        {
            using (var client = new ClientProvider().Client)
            {
                string param = string.Join("&", users.Select(x => "usernames=" + x));
                var response = await client.GetAsync($"api/github/gitusers/?{ param }");
                var gitusers = JsonConvert.DeserializeObject<List<Users>>(response.Content.ReadAsStringAsync().Result);

                Assert.Equal(2, gitusers.Count);
            }
        }

        [Theory]
        [InlineData(new object[] { new string[] { "jonsnow1144", "caged", "kevinclark", "anotherjesse" } })]
        public async Task GitUsers_TestSorting(string[] users)
        {
            using (var client = new ClientProvider().Client)
            {
                string param = string.Join("&", users.Select(x => "usernames=" + x));
                var response = await client.GetAsync($"api/github/gitusers/?{ param }");
                var gitusers = JsonConvert.DeserializeObject<List<Users>>(response.Content.ReadAsStringAsync().Result);

                //Sorted by name
                //-----------------------
                //jonsnow1144 - Carl                
                //Names - Jesse Andrews
                //caged - Justin Palmer                                
                //kevinclark - Kevin Clark

                Assert.Equal("Carl", gitusers[0].Name);                
                Assert.Equal("Jesse Andrews", gitusers[1].Name);
                Assert.Equal("Justin Palmer", gitusers[2].Name);
                Assert.Equal("Kevin Clark", gitusers[3].Name);
            }
        }
    }
}
