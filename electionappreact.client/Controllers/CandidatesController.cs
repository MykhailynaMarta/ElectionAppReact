using ElectionApp.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ElectionApp.Controllers
{
    public class CandidatesController : Controller
    {
        private static List<Candidate> candidates = new List<Candidate>
        {
            new Candidate { Id = 1, Name = "Oleh Petrenko", Party = "Green Party", Deputy="XII", Description="Born: 1982" },
            new Candidate { Id = 2, Name = "Petro Loval", Party = "Democratic Party",  Deputy="XI", Description="Born: 1893" },
        };
        public IActionResult CandidatesView()
        {
            return View(candidates);
        }
        public IActionResult CandidateInfoView(int id)
        {
            var candidate = candidates.FirstOrDefault(c => c.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            // Повертаємо сторінку з одним кандидатом
            return View(candidate);
        }

    }

}
