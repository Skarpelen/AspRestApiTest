using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspRestApiTest.Areas.User.Controllers.Journal
{
    using AspRestApiTest.Areas.User.Models;
    using AspRestApiTest.Areas.User.Schemas;
    using AspRestApiTest.Data;

    [Area("User")]
    [Route("api/[area]/[controller]")]
    public class JournalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JournalController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("getRange")]
        public async Task<IActionResult> GetRange([FromQuery] int skip, [FromQuery] int take, [FromBody] VJournalFilter filter)
        {
            var query = _context.ExceptionJournals.AsQueryable();

            if (filter.From.HasValue)
            {
                query = query.Where(j => j.Timestamp >= filter.From.Value);
            }

            if (filter.To.HasValue)
            {
                query = query.Where(j => j.Timestamp <= filter.To.Value);
            }

            if (!string.IsNullOrEmpty(filter.Search))
            {
                if (long.TryParse(filter.Search, out var searchId))
                {
                    query = query.Where(j => j.EventId == searchId);
                }
            }

            var journals = await query.Skip(skip).Take(take).ToListAsync();
            var result = new MRange<MJournalInfo>
            {
                Skip = skip,
                Count = await query.CountAsync(),
                Items = journals.Select(j => new MJournalInfo
                {
                    Id = j.Id,
                    EventId = j.EventId,
                    CreatedAt = j.Timestamp
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost("getSingle")]
        public async Task<IActionResult> GetSingle([FromQuery] long id)
        {
            var journal = await _context.ExceptionJournals.FindAsync(id);

            if (journal == null)
            {
                return NotFound(new { Message = $"Journal entry with ID {id} not found." });
            }

            var result = new MJournal
            {
                Id = journal.Id,
                EventId = journal.EventId,
                CreatedAt = journal.Timestamp,
                Text = journal.StackTrace
            };

            return Ok(result);
        }
    }
}
