using Microsoft.AspNetCore.Mvc;

namespace ElectionApp.Controllers
{
    public class HelloController : Controller
    {
        public IActionResult HelloWorld()
        {
            return View();
        }
    }
}
