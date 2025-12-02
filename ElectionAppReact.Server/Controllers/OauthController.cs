using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ElectionAppReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OauthController : ControllerBase
    {
        private readonly IHttpClientFactory _http;

        public OauthController(IHttpClientFactory http)
        {
            _http = http;
        }

        [HttpPost("callback")]
        public async Task<IActionResult> Callback([FromBody] CallbackRequest request)
        {
            if (string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.Verifier))
                return BadRequest("Missing code or verifier");

            var client = _http.CreateClient();

            var body = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", "react-client" },
                { "code", request.Code },
                { "redirect_uri", "http://localhost:5173/callback" },
                { "code_verifier", request.Verifier }
            });

            var response = await client.PostAsync("http://localhost:5001/connect/token", body);

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode(500, json);

            var parsed = JsonSerializer.Deserialize<object>(json);
            return Ok(parsed);
        }
    }

    public class CallbackRequest
    {
        public string Code { get; set; }
        public string Verifier { get; set; }
    }
}
