using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspRestApiTest.Areas.User.Controllers.Tree.Node
{
    using AspRestApiTest.Areas.User.Models;
    using AspRestApiTest.Data;

    [Area("User")]
    [Route("api/[area]/[controller]")]
    public class NodeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNode([FromQuery] string treeName, [FromQuery] long parentNodeId, [FromQuery] string nodeName)
        {
            var tree = await _context.Trees.FirstOrDefaultAsync(t => t.Name == treeName);

            if (tree is null)
            {
                return NotFound(new { Message = $"Tree with name {treeName} not found." });
            }

            var parentNode = await _context.Nodes.FirstOrDefaultAsync(n => n.Id == parentNodeId && n.TreeId == tree.Id);

            if (parentNode is null)
            {
                return BadRequest(new { Message = $"Parent node {parentNodeId} not found in tree {treeName}." });
            }

            var newNode = new Data.Models.Node
            {
                Name = nodeName,
                ParentNodeId = parentNode.Id,
                TreeId = tree.Id
            };

            _context.Nodes.Add(newNode);
            await _context.SaveChangesAsync();

            var result = new MNode
            {
                Id = newNode.Id,
                Name = newNode.Name,
                Children = new List<MNode>()
            };

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteNode([FromQuery] string treeName, [FromQuery] long nodeId)
        {
            var node = await _context.Nodes
                .Include(n => n.ChildNodes)
                .FirstOrDefaultAsync(n => n.Id == nodeId && n.Tree.Name == treeName);

            if (node is null)
            {
                return NotFound(new { Message = $"Node {nodeId} in tree {treeName} not found." });
            }

            if (node.ChildNodes.Any())
            {
                return BadRequest(new { Message = "Cannot delete a node that has children. Delete child nodes first." });
            }

            _context.Nodes.Remove(node);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Node {nodeId} in tree {treeName} deleted" });
        }

        [HttpPost("rename")]
        public async Task<IActionResult> RenameNode([FromQuery] string treeName, [FromQuery] long nodeId, [FromQuery] string newNodeName)
        {
            var node = await _context.Nodes
                .FirstOrDefaultAsync(n => n.Id == nodeId && n.Tree.Name == treeName);

            if (node is null)
            {
                return NotFound(new { Message = $"Node {nodeId} in tree {treeName} not found." });
            }

            node.Name = newNodeName;
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"Node {nodeId} renamed to {newNodeName} in tree {treeName}" });
        }
    }
}
