using Microsoft.AspNetCore.Mvc;

namespace AspRestApiTest.Areas.User.Controllers.Journal
{
    using AspRestApiTest.Areas.User.Models;

    [Area("User")]
    public class JournalController : Controller
    {
        [HttpPost("getRange")]
        public IActionResult GetRange([FromQuery] int skip, [FromQuery] int take, [FromBody] VJournalFilter filter)
        {
            var result = new MRange<MJournalInfo>
            {
                Skip = skip,
                Count = take,
                Items = new List<MJournalInfo>
            {
                new MJournalInfo
                {
                    Id = 1,
                    EventId = 100,
                    CreatedAt = DateTime.UtcNow
                }
            }
            };

            return Ok(result);
        }

        [HttpPost("getSingle")]
        public IActionResult GetSingle([FromQuery] long id)
        {
            var journal = new MJournal
            {
                Text = "Example text",
                Id = id,
                EventId = 100,
                CreatedAt = DateTime.UtcNow
            };

            return Ok(journal);
        }
    }
}
