using ElectionAppReact.Server.Data;
using ElectionAppReact.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectionAppReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IBankDbContextFactory _factory;

        public AuthorizationController(IBankDbContextFactory factory)
        {
            _factory = factory;
        }

        // ГОЛОВНА АВТОРИЗАЦІЯ
        [HttpPost("check")]
        public async Task<IActionResult> CheckUser([FromBody] UserLoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Bank) ||
                string.IsNullOrEmpty(dto.FullName) ||
                string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("Missing fields");
            }

            var bank = dto.Bank.ToLower();

            // ---------- ADMIN LOGIN ----------
            if (bank == "admin")
            {
                // Адміни зберігаються у MONOBANK PostgreSQL
                var adminDb = _factory.Create("monobank");

                var admin = await adminDb.AdminUsers
                    .FirstOrDefaultAsync(x =>
                        x.FullName == dto.FullName &&
                        x.Password == dto.Password);

                if (admin != null)
                    return Ok(new { status = "admin-valid" });

                return Unauthorized();
            }

            // ---------- NORMAL USER ----------
            var db = _factory.Create(bank);

            var user = await db.BankUsers
                .FirstOrDefaultAsync(x =>
                    x.FullName == dto.FullName &&
                    x.Password == dto.Password);

            if (user != null)
                return Ok(new { status = "valid" });

            return Unauthorized();
        }

        // ---------- PKCE login URL ----------
        [HttpGet("login-url")]
        public IActionResult GetLoginUrl([FromQuery] string codeChallenge)
        {
            if (string.IsNullOrEmpty(codeChallenge))
                return BadRequest("Missing codeChallenge");

            string url =
                "http://localhost:5001/connect/authorize?" +
                "client_id=react-client&" +
                "redirect_uri=http://localhost:5173/callback&" +
                "response_type=code&" +
                "scope=openid%20profile%20electionapi&" +
                $"code_challenge={codeChallenge}&" +
                "code_challenge_method=S256";

            return Ok(url);
        }
    }

    // DTO
    public class UserLoginDto
    {
        public string Bank { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
