using System.ComponentModel.DataAnnotations;

namespace AspRestApiTest.Areas.User.Models
{
    public class MNode
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public List<MNode> Children { get; set; } = new List<MNode>();
    }
}
