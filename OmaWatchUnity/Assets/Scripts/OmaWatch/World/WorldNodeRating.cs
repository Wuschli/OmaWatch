using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.World
{
    public class WorldNodeRating
    {
        public WorldNode Node { get; }
        public WorldNodeRating Predecessor { get; set; }
        public float Rating { get; set; }
        public float Cost { get; set; }

        public WorldNodeRating(WorldNode node)
        {
            Node = node;
            Rating = float.PositiveInfinity;
            Cost = float.PositiveInfinity;
        }

        public void GetPath(ref List<Vector3> path, Grid grid)
        {
            path.Insert(0, grid.GetCellCenterWorld(Node.Pos));
            Predecessor?.GetPath(ref path, grid);
        }
    }
}