using Marathon.Data;
using Marathon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace Marathon.Controllers
{
	public class UserHomeController : Controller
	{
        private readonly MarathonContext _context;

        public UserHomeController(MarathonContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var marathon = await _context.Marathonentity.ToListAsync();
            var marathonorder= marathon.OrderByDescending(a=>a.MarathonData).Take(3).ToList();
            return View(marathonorder);
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
