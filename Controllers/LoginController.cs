using marathon.Models;
using Marathon.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Marathon.Controllers
{
    public class LoginController : Controller
    {
        private readonly MarathonContext _context;

        public LoginController(MarathonContext context)
        {
            _context = context;
        }
        public ViewResult back()
        {
            return View("Index");
        }
        public ViewResult reg()
		{
			ViewBag.Gender = new SelectList(Enum.GetValues(typeof(Gender)).Cast<Gender>());
			return View("reg");
		}
		[HttpGet]
        public ViewResult Index()
        {
            ViewBag.Login = "登录";
            return View("Index");
        }



        //检验登录信息用户
        [HttpPost]
        public async Task<ActionResult> Index(User user)
        {
            var _manager  = await _context.User
                .FirstOrDefaultAsync(u => u.UserName == user.UserName && u.PassWord == user.PassWord);
            if (_manager==null)
            {
                ViewBag.Login = "登录失败！";
                return View();
            }
            else
            {
                ViewBag.username=user.UserName;
                HttpContext.Session.SetString("User", _manager.UserName);
                return RedirectToAction("Index", "UserHome");
            }
        }
		[HttpGet]
		public ViewResult managerlog()
		{
			ViewBag.Login = "登录";
			return View("managerlog");
		}
		//检验登录信息
		[HttpPost]
		public async Task<IActionResult> managerlog(Manager manager)
		{
			var _manager = await _context.Manager
				.FirstOrDefaultAsync(u => u.ManagerId == manager.ManagerId && u.PassWord == manager.PassWord);
			if (_manager == null)
			{
				ViewBag.Login = "登录失败！";
				return View();
			}
			else
			{
				HttpContext.Session.SetString("User", _manager.ManagerName);
				return View("~/Views/Home/Index.cshtml", _manager);
			}
		}

		//保存注册信息
		public async Task<IActionResult> Create([Bind("UserName,PassWord,Age,PhoneNumber,Name")] User user, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    byte[] avatarData = new byte[image.Length];
                    await image.CopyToAsync(new MemoryStream(avatarData));
                    user.Image = avatarData;
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
