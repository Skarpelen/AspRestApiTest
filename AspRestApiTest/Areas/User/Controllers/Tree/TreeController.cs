using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspRestApiTest.Areas.User.Controllers.Tree
{
    using AspRestApiTest.Areas.User.Models;
    using AspRestApiTest.Data;
    using AspRestApiTest.Features.Exceptions;

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
            if (string.IsNullOrWhiteSpace(treeName))
            {
                throw new SecureException("Name of the tree must be filled.");
            }

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

                return Ok(rootNode.ToMNode());
            }

            var existingRootNode = await _context.Nodes
                .Where(n => n.TreeId == tree.Id && n.ParentNodeId == null)
                .FirstOrDefaultAsync();

            if (existingRootNode is null)
            {
                throw new SecureException($"Root node for tree {treeName} not found.");
            }

            var result = await LoadFullNodeHierarchy(existingRootNode);

            return Ok(result);
        }

        private async Task<MNode> LoadFullNodeHierarchy(Data.Models.Node node)
        {
            var mNode = node.ToMNode();

            var childNodes = await _context.Nodes
                .Where(n => n.ParentNodeId == node.Id)
                .ToListAsync();

            foreach (var childNode in childNodes)
            {
                var childMNode = await LoadFullNodeHierarchy(childNode);
                mNode.Children.Add(childMNode);
            }

            return mNode;
        }
    }
}
