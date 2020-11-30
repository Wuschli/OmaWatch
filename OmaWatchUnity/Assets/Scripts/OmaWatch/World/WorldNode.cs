using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.World
{
    public class WorldNode
    {
        public Vector3Int Pos { get; }
        public FloorTag Tag { get; set; }
        public IEnumerable<WorldNode> Connections => _connections;

        private readonly List<WorldNode> _connections = new List<WorldNode>();

        public WorldNode(Vector3Int pos, FloorTag tag)
        {
            Pos = pos;
            Tag = tag;
        }

        public void AddConnection(WorldNode node)
        {
            if (!_connections.Contains(node))
                _connections.Add(node);
            if (!node._connections.Contains(this))
                node._connections.Add(this);
        }

        public void UnConnect()
        {
            foreach (var connection in _connections)
                connection._connections.Remove(this);
        }
    }
    
}