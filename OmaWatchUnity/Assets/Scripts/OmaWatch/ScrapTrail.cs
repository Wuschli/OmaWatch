using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.GamePlay.Interactions;
using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    [RequireComponent(typeof(LineRenderer))]
    public class ScrapTrail : MonoBehaviour
    {
        public int MaxElementCount = 1;
        public int MaxTrailLength = 512;
        public int TrailSpacing = 16;
        public int TrailStartSpacing = 64;

        public TrailElementRenderer ElementPrefab;
        public ScrapPiece DroppedPrefab;

        private readonly List<Vector3> _trail = new List<Vector3>();
        private LineRenderer _lineRenderer;
        public bool HasScrap => transform.childCount > 0;

        public bool TryAddElement(ScrapPieceConfig config)
        {
            if (transform.childCount >= MaxElementCount)
                return false;
            var newElement = Instantiate(ElementPrefab, transform, false);
            newElement.Config = config;
            return true;
        }

        public ScrapPieceConfig TakeScrap()
        {
            var child = transform.GetChild(0).gameObject;
            var result = child.GetComponent<TrailElementRenderer>().Config;
            Destroy(child);
            return result;
        }

        public void DropAll()
        {
            foreach (var child in GetComponentsInChildren<TrailElementRenderer>())
                Drop(child);
        }


        protected void OnEnable()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        protected virtual void Update()
        {
            while (_trail.Count >= MaxTrailLength)
                _trail.RemoveAt(_trail.Count - 1);

            if (_trail.Any() && _trail.First() == transform.position)
                return;

            _trail.Insert(0, transform.position);
            var lastPosition = transform.position;
            var i = 0;
            foreach (Transform child in transform)
            {
                var index = i++ * TrailSpacing + TrailStartSpacing;
                if (index > _trail.Count)
                    index = _trail.Count - 1;
                if (index > 0)
                    lastPosition = _trail[index];

                child.position = lastPosition;
            }

            _lineRenderer.positionCount = _trail.Count;
            _lineRenderer.SetPositions(_trail.ToArray());
        }

        protected virtual void Drop(TrailElementRenderer element)
        {
            var config = element.Config;
            var position = element.transform.position;
            Instantiate(DroppedPrefab, position, Quaternion.identity).Config = config;
            Destroy(element.gameObject);
        }
    }
}