    using ElectionApp.Models;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    namespace ElectionApp.Controllers
    {
        public class AuthorizationController : Controller
        {
            public static List<User> Users = LoadUsers();
            public static List<User> LoadUsers()
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
                var json = System.IO.File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<User>>(json);
            }
            [HttpGet]
            public IActionResult AuthorizationForm()
            {
                return View();
            }

        [HttpPost]
        public IActionResult AuthorizationForm(string bank, string password, IFormFile keyFile)
        {
            if (keyFile == null || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(bank))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля та завантажте ключ!";
                return View();
            }

            // Тут ти можеш зчитати файл keyFile та перевірити його вміст (для простоти поки порівнюємо ім'я файлу)
            var uploadedFileName = Path.GetFileName(keyFile.FileName);

            // Шукаємо користувача у JSON по банку і "пін" (тимчасово PIN може відповідати Password)
            var user = Users.FirstOrDefault(u => u.Bank == bank && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("UserPageView", "User", new { id = user.Id });
            }

            ViewBag.Error = "Невірний ключ, PIN або банк!";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

    }
