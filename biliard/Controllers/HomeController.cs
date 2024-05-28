using biliard.Data;
using biliard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace biliard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {
            int users = _context.User
                .Count();
            int tables = _context.Table
                .Count();
            int reservations = _context.Reservation
                .Count();
            ;

            ViewData["Reservations"] = reservations;
            ViewData["Tables"] = tables;
            ViewData["Users"] = users;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}