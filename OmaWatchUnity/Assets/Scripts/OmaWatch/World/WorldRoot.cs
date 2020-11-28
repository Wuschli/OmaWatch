using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.World
{
    [RequireComponent(typeof(Grid))]
    public class WorldRoot : MonoBehaviour
    {
        public static WorldRoot Instance { get; private set; }

        private Grid _tileGrid;
        private readonly List<WorldNode> _nodes = new List<WorldNode>();

        private void Awake()
        {
            Instance = this;

            _tileGrid = GetComponent<Grid>();


            foreach (var floor in GetComponentsInChildren<FloorTag>())
            {
                var tilemap = floor.Tilemap;
                foreach (var pos in tilemap.cellBounds.allPositionsWithin)
                {
                    var tile = tilemap.GetTile(pos);
                    if (tile == null)
                        continue;
                    if (_nodes.Any(n => n.Pos == pos))
                        continue;

                    var node = new WorldNode(pos, floor);

                    _nodes.Add(node);
                    TryAddConnectionNode(pos + Vector3Int.left, node);
                    TryAddConnectionNode(pos + Vector3Int.right, node);
                    TryAddConnectionNode(pos + Vector3Int.up, node);
                    TryAddConnectionNode(pos + Vector3Int.down, node);
                }
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void TryAddConnectionNode(Vector3Int pos, WorldNode node)
        {
            var connection = _nodes.FirstOrDefault(n => n.Pos == pos);
            connection?.AddConnection(node);
        }

        public bool IsTileSafe(Vector3 worldPos)
        {
            var tilePos = GetTilePos(ClampToTile(worldPos));
            var node = _nodes.FirstOrDefault(n => n.Pos == tilePos);
            if (node == null || node.Tag.Type == FloorTag.FloorType.Safe)
                return true;
            return false;
        }

        public Vector3Int GetTilePos(Vector3 pos)
        {
            return _tileGrid.WorldToCell(pos);
        }

        public Vector3 ClampToTile(Vector3 pos)
        {
            return _tileGrid.GetCellCenterWorld(GetTilePos(pos));
        }

        public Vector3 GetTileWorldPos(Vector3Int tile)
        {
            return _tileGrid.CellToWorld(tile);
        }

        public List<Vector3> GetPath(Vector3 from, Vector3 to)
        {
            var open = new List<WorldNodeRating>();
            var closed = new List<WorldNodeRating>();

            var fromPos = _tileGrid.WorldToCell(from);
            var toPos = _tileGrid.WorldToCell(to);

            if (fromPos == toPos)
                return null;

            var fromNode = _nodes.FirstOrDefault(n => n.Pos == fromPos);
            var toNode = _nodes.FirstOrDefault(n => n.Pos == toPos);

            if (fromNode == null || toNode == null)
                return null;

            open.Add(new WorldNodeRating(fromNode) {Cost = 0, Rating = 0});
            while (open.Count > 0)
            {
                var current = open[0];
                open.RemoveAt(0);

                if (current.Node.Pos == toPos)
                {
                    var result = new List<Vector3>();
                    current.GetPath(ref result, _tileGrid);
                    return result.Skip(1).ToList();
                }

                closed.Add(current);
                Expand(current, open, closed, toPos);

                open = open.OrderBy(o => o.Rating).ToList();
            }

            Debug.LogWarning($"path from {from} to {to} was not found.");
            return null;
        }

        private void Expand(WorldNodeRating current, List<WorldNodeRating> open, List<WorldNodeRating> closed, Vector3Int target)
        {
            foreach (var connection in current.Node.Connections)
            {
                if (closed.Any(c => c.Node == connection))
                    continue;

                var cost = current.Cost + 1;
                var connectionRating = open.FirstOrDefault(c => c.Node == connection);
                if (connectionRating != null && connectionRating.Cost < cost)
                    continue;
                if (connectionRating == null)
                    connectionRating = new WorldNodeRating(connection);

                connectionRating.Predecessor = current;
                connectionRating.Cost = cost;

                var rating = cost + Vector3Int.Distance(connection.Pos, target);
                if (rating <= connectionRating.Rating)
                    connectionRating.Rating = rating;

                if (!open.Contains(connectionRating))
                    open.Add(connectionRating);
            }
        }
    }
}
