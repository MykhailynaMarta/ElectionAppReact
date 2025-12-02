using ElectionAppReact.Server.Data;
using ElectionAppReact.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectionAppReact.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ElectionDbContext _db;

        public CandidatesController(ElectionDbContext db)
        {
            _db = db;
        }

        // GET api/candidates
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Candidates.ToListAsync();
            return Ok(list);
        }

        // DELETE api/candidates/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Candidates.FindAsync(id);
            if (item == null)
                return NotFound();

            _db.Candidates.Remove(item);
            await _db.SaveChangesAsync();

            return Ok(new { message = "deleted" });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var candidate = await _db.Candidates.FindAsync(id);

            if (candidate == null)
                return NotFound();

            return Ok(candidate);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Candidate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _db.Candidates.Add(model);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.id }, model);
        }
        // PUT api/candidates/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Candidate model)
        {
            if (id != model.id)
                return BadRequest("Id mismatch");

            var candidate = await _db.Candidates.FindAsync(id);
            if (candidate == null)
                return NotFound();

            candidate.name = model.name;
            candidate.description = model.description;
            candidate.party = model.party;
            candidate.birthdate = model.birthdate;
            candidate.photourl = model.photourl;

            await _db.SaveChangesAsync();

            return Ok(candidate);
        }

    }
}
