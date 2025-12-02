using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ElectionAppReact.Server.Controllers
{
    [Route("api/voting")]
    [ApiController]
    public class VotingApiController : ControllerBase
    {
        [HttpPost("vote")]
        public IActionResult Vote([FromBody] VoteRequest request)
        {
            if (request == null || request.CandidateId == 0)
                return BadRequest(new { error = "invalid data" });

            string hash = ComputeHash(request.CandidateId);

            Console.WriteLine($"Vote: {request.CandidateId} => HASH = {hash}");

            return Ok(new
            {
                status = "ok",
                hash = hash
            });
        }


        private string ComputeHash(int candidateId)
        {
            string plain = $"v-{candidateId}-{DateTime.UtcNow.Ticks}";
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(plain));
                return Convert.ToBase64String(bytes);
            }
        }
    }

    public class VoteRequest
    {
        public int CandidateId { get; set; }
    }
}
