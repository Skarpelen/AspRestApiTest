using AspRestApiTest.Areas.User.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspRestApiTest.Areas.User.Controllers.Tree.Node
{
    [Area("User")]
    public class NodeController : Controller
    {
        [HttpPost("create")]
        public IActionResult CreateNode([FromQuery] string treeName, [FromQuery] long parentNodeId, [FromQuery] string nodeName)
        {
            var newNode = new MNode
            {
                Id = 3,
                Name = nodeName
            };

            return Ok(newNode);
        }

        [HttpPost("delete")]
        public IActionResult DeleteNode([FromQuery] string treeName, [FromQuery] long nodeId)
        {
            return Ok(new { Message = $"Node {nodeId} in tree {treeName} deleted" });
        }

        [HttpPost("rename")]
        public IActionResult RenameNode([FromQuery] string treeName, [FromQuery] long nodeId, [FromQuery] string newNodeName)
        {
            return Ok(new { Message = $"Node {nodeId} renamed to {newNodeName} in tree {treeName}" });
        }
    }
}
