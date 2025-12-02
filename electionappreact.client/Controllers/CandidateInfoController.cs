using Microsoft.AspNetCore.Mvc;
using ElectionApp.Models;

namespace ElectionApp.Controllers
{
    public class CandidateInfoController : Controller
    {
        public IActionResult CandidateInfoView(int Id)
        {
            return View();
        }
    }
}
