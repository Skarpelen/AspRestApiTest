using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspRestApiTest.Areas.User.Controllers.Tree
{
    using AspRestApiTest.Areas.User.Models;
    using AspRestApiTest.Data;

    [Area("User")]
    [Route("api/[area]/[controller]")]
    public class TreeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetTree([FromQuery] string treeName)
        {
            var tree = await _context.Trees.FirstOrDefaultAsync(t => t.Name == treeName);

            if (tree is null)
            {
                tree = new Data.Models.Tree
                {
                    Name = treeName
                };

                _context.Trees.Add(tree);
                await _context.SaveChangesAsync();

                var rootNode = new Data.Models.Node
                {
                    Name = treeName + " Root",
                    TreeId = tree.Id
                };

                _context.Nodes.Add(rootNode);
                await _context.SaveChangesAsync();

                return Ok(ConvertToMNode(rootNode));
            }

            var existingRootNode = await _context.Nodes
                .Where(n => n.TreeId == tree.Id && n.ParentNodeId == null)
                .Include(n => n.ChildNodes)
                .FirstOrDefaultAsync();

            if (existingRootNode is null)
            {
                return NotFound(new { Message = $"Root node for tree {treeName} not found." });
            }

            return Ok(ConvertToMNode(existingRootNode));
        }

        private MNode ConvertToMNode(Data.Models.Node node)
        {
            return new MNode
            {
                Id = node.Id,
                Name = node.Name,
                Children = node.ChildNodes?.Select(ConvertToMNode).ToList() ?? new List<MNode>()
            };
        }
    }
}
