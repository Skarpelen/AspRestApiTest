using System.ComponentModel.DataAnnotations;

namespace AspRestApiTest.Areas.User.Models
{
    public class MJournalInfo
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public long EventId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
