using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using ElectionApp.Models;

namespace ElectionApp.Controllers
{
    public class VotingController : Controller
    {
        // GET: /Voting/VotingView
        [HttpGet]
        public IActionResult VotingView()
        {
            // Підвантажуємо кандидатів
            var candidates = GetCandidates();
            return View(candidates);
        }

        // Метод для отримання кандидатів (можна замінити на БД або JSON)
        private List<Candidate> GetCandidates()
        {
            return new List<Candidate>
            {
                new Candidate { Id = 1, Name = "Oleh Petrenko", Party = "Green Party", Deputy="XII", Description="Born: 1982" },
                new Candidate { Id = 2, Name = "Petro Ivanov", Party = "Blue Party", Deputy="XII", Description="Born: 1985" }
                // Додати інших кандидатів
            };
        }
        [HttpPost]
        public IActionResult Submit(int selectedCandidateId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized();

            var voteHash = EncryptVote(userId, selectedCandidateId);
            SaveVote(userId, voteHash);

            return View("VotingConfirmationView", voteHash);
        }

        // --- Допоміжні методи ---
        private int GetCurrentUserId()
        {
            if (HttpContext.Session.TryGetValue("UserId", out var data))
            {
                return BitConverter.ToInt32(data);
            }
            return 0;
        }

        private string EncryptVote(int userId, int candidateId)
        {
            var plainText = $"{userId}-{candidateId}";
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(plainText);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private void SaveVote(int userId, string voteHash)
        {
            // Зберігаємо в БД, JSON або пам'ять
            Console.WriteLine($"User {userId} voted: {voteHash}");
        }
    }
}