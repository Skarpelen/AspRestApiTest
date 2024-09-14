using AspRestApiTest.Areas.User.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspRestApiTest.Areas.User.Controllers.Tree
{
    [Area("User")]
    public class TreeController : Controller
    {
        [HttpPost("get")]
        public IActionResult GetTree([FromQuery] string treeName)
        {
            var tree = new MNode
            {
                Id = 1,
                Name = treeName,
                Children = new List<MNode>
            {
                new MNode
                {
                    Id = 2,
                    Name = "ChildNode"
                }
            }
            };

            return Ok(tree);
        }
    }
}
