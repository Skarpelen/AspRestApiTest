using System.ComponentModel.DataAnnotations;

namespace AspRestApiTest.Areas.User.Models
{
    public class VJournalFilter
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        [Required]
        public string Search { get; set; }
    }

}
