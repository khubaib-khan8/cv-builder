// Controllers/CVController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CVBuilderAPI.Data;
using CVBuilderAPI.Models;

namespace CVBuilderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]                          // ← All endpoints require JWT
    public class CVController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CVController(AppDbContext context) { _context = context; }

        // Helper: get logged-in user's ID from JWT
        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // ── GET api/cv  (only current user's CVs) ────────────
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CV>>> GetAll()
        {
            var userId = GetUserId();
            return await _context.CVs
                .Where(c => c.UserId == userId)
                .Include(c => c.Experiences)
                .Include(c => c.Educations)
                .Include(c => c.Skills)
                .ToListAsync();
        }

        // ── GET api/cv/{id} ───────────────────────────────────
        [HttpGet("{id}")]
        public async Task<ActionResult<CV>> GetById(int id)
        {
            var userId = GetUserId();
            var cv = await _context.CVs
                .Include(c => c.Experiences)
                .Include(c => c.Educations)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cv == null) return NotFound();
            return cv;
        }

        // ── POST api/cv ───────────────────────────────────────
        [HttpPost]
        public async Task<ActionResult<CV>> Create(CV cv)
        {
            cv.UserId    = GetUserId();
            cv.CreatedAt = DateTime.UtcNow;

            _context.CVs.Add(cv);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cv.Id }, cv);
        }

        // ── PUT api/cv/{id} ───────────────────────────────────
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CV cv)
        {
            var userId = GetUserId();

            if (id != cv.Id) return BadRequest();

            // Make sure CV belongs to this user
            var existing = await _context.CVs
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (existing == null) return NotFound();

            cv.UserId = userId;
            _context.Entry(existing).CurrentValues.SetValues(cv);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ── DELETE api/cv/{id} ────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var cv = await _context.CVs
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (cv == null) return NotFound();
            _context.CVs.Remove(cv);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}