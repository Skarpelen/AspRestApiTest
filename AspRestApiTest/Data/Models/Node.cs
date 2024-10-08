﻿using System.ComponentModel.DataAnnotations;

namespace AspRestApiTest.Data.Models
{
    using AspRestApiTest.Areas.User.Models;

    public class Node
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int? ParentNodeId { get; set; }

        public int TreeId { get; set; }

        public Node ParentNode { get; set; }

        public ICollection<Node> ChildNodes { get; set; }

        public Tree Tree { get; set; }

        public MNode ToMNode()
        {
            return new MNode
            {
                Id = Id,
                Name = Name,
                Children = new List<MNode>()
            };
        }
    }
}
