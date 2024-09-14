using System.ComponentModel.DataAnnotations;

namespace AspRestApiTest.Areas.User.Models
{
    public class MRange<T>
    {
        [Required]
        public int Skip { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public List<T> Items { get; set; }
    }

}
