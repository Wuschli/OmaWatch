using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.World;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class TileNavMovementController : MonoBehaviour
    {
        [Range(0.1f, 5f)] public float Speed = 1f;
        public Transform DebugTargetPos;
        public GameObject TileIndicator;

        private TaskCompletionSource<MoveResult> _currentOp;
        private readonly Queue<Vector3> _movePositions = new Queue<Vector3>();



        private float _time;
        private Vector3 _prevPos;
        private Transform _currentTarget;
        private Vector3Int _currentTargetTile;


        private void Start()
        {
            if (DebugTargetPos != null)
                MoveToTarget(DebugTargetPos);
        }

        public Task<MoveResult> MoveToTarget(Transform target)
        {
            _currentOp?.SetResult(MoveResult.Canceled);

            _currentTarget = target;
            var position = target.position;
            _currentTargetTile = WorldRoot.Instance.GetTilePos(position);
            var path = WorldRoot.Instance.GetPath(transform.position, position);
            if (path == null)
                return Task.FromResult(MoveResult.Failed);

            _movePositions.Clear();
            path.ForEach(p => _movePositions.Enqueue(p));
            _prevPos = transform.position;

            _currentOp = new TaskCompletionSource<MoveResult>();
            return _currentOp.Task;
        }

        public void Stop()
        {
            _currentOp?.SetResult(MoveResult.Canceled);
            _movePositions.Clear();
            _currentTarget = null;
        }


        private void Update()
        {
            if (TileIndicator != null)
                TileIndicator.transform.position = WorldRoot.Instance.ClampToTile(transform.position);

            if (_movePositions.Count == 0)
                return;

            _time += Time.deltaTime * Speed;

            transform.position = Vector3.Lerp(_prevPos, _movePositions.Peek(), _time);
            if (_time < 1f)
                return;

            _prevPos = _movePositions.Dequeue();
            _time = _time - 1f;

            //target has moved, repath!
            var tileNow = WorldRoot.Instance.GetTilePos(_currentTarget.position);
            if (tileNow != _currentTargetTile)
            {
                _currentTargetTile = tileNow;
                var newPath = WorldRoot.Instance.GetPath(_prevPos, _currentTarget.position);

                _movePositions.Clear();
                newPath.ForEach(p => _movePositions.Enqueue(p));
            }

            if (_movePositions.Count == 0)
            {
                _currentOp?.SetResult(MoveResult.Success);
                _currentOp = null;
            }
        }

        public enum MoveResult
        {
            Success,
            Failed,
            Canceled
        }
    }
}