using ElectionApp.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using System.Text.Json;

namespace ElectionApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserPageView(int id)
        {
            var user = AuthorizationController.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }
    }
}
