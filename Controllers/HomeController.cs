using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using WebApp_OpenIDConnect_DotNet.Models;

namespace WebApp_OpenIDConnect_DotNet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly ITokenAcquisition tokenAcquisition;

        public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            this.tokenAcquisition = tokenAcquisition;
        }

        public async Task<IActionResult> Index()
        {
            // Acquire the access token.
            string[] scopes = new string[] { "api://xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/user.read" };
            string accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);

            var httpClient = new HttpClient();
            //
           

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);            
            var response = await httpClient.GetAsync("https://some URL");
            var stringResponse = await response.Content.ReadAsStringAsync();

            Console.WriteLine(accessToken);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
