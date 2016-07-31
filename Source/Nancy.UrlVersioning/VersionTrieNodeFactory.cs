using System.Collections.Generic;
using Nancy.Routing.Constraints;
using Nancy.Routing.Trie;
using Nancy.Routing.Trie.Nodes;

namespace Nancy.UrlVersioning
{
    /// <summary>
    /// Factory for creating the correct type of TrieNode
    /// </summary>
    public class VersionTrieNodeFactory : TrieNodeFactory
    {
        private readonly IVersionInfoFactory _factory;

        public VersionTrieNodeFactory(IEnumerable<IRouteSegmentConstraint> routeSegmentConstraints, IVersionInfoFactory factory = null)
            : base(routeSegmentConstraints)
        {
            _factory = factory;
        }

        /// <summary>
        /// Gets the correct Trie node type for the given segment
        /// </summary>
        /// <param name="parent">Parent node</param>
        /// <param name="segment">Route segment</param>
        /// <returns>TrieNode instance</returns>
        public override TrieNode GetNodeForSegment(TrieNode parent, string segment)
        {
            if (parent == null)
            {
                return new RootNode(this);
            }

            if (VersionNode.Supports(segment))
            {
                return new VersionNode(parent, segment, this, _factory);
            }

            return base.GetNodeForSegment(parent, segment);
        }
    }
}
