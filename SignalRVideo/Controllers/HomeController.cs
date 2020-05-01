using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRVideo.Hubs;
using SignalRVideo.Models;

namespace SignalRVideo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<VideoHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, IHubContext<VideoHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        string rootPath = Directory.GetCurrentDirectory();
        public async Task<IActionResult> StartSendingFrames()
        {
            byte[] imageFile = null;
            string[] dir = Directory.GetFiles(Path.Combine(rootPath, @"SamplePics\"), "*.jpg");
            while (true)
            { 
                foreach (var file in dir)
                {
                    imageFile = System.IO.File.ReadAllBytes(file);
                    await _hubContext.Clients.All.SendAsync("initSignal", "data: image / png; base64,"
                        + Convert.ToBase64String(imageFile));
                    Thread.Sleep(1000);
                }
            }
            return Json(true);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
