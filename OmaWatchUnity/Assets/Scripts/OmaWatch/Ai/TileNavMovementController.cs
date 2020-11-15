using System;
using System.Collections.Generic;
using System.Threading;
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
        private CancellationToken? _currentCancellationToken;
        private readonly Queue<Vector3> _movePositions = new Queue<Vector3>();


        private float _time;
        private Vector3 _prevPos;
        private Transform _currentTarget;
        private Vector3Int _currentTargetTile;


        private void Start()
        {
            if (DebugTargetPos != null)
                MoveToTarget(DebugTargetPos, CancellationToken.None);
        }

        public Task<MoveResult> MoveToTarget(Transform target, CancellationToken cancellationToken)
        {
            if(_currentOp != null)
                CompleteOp(MoveResult.Canceled);

            _currentTarget = target;
            _currentCancellationToken = cancellationToken;

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
            CompleteOp(MoveResult.Canceled);
        }

        private void Update()
        {
            if (TileIndicator != null)
                TileIndicator.transform.position = WorldRoot.Instance.ClampToTile(transform.position);

            if (_currentCancellationToken.HasValue && _currentCancellationToken.Value.IsCancellationRequested)
            {
                Stop();
                return;
            }


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
                CompleteOp(MoveResult.Success);
        }

        private void CompleteOp(MoveResult result)
        {
            var op = _currentOp;

            _currentOp = null;
            _currentCancellationToken = null;
            _currentTarget = null;

            _movePositions.Clear();


            op?.SetResult(result);
        }

        public enum MoveResult
        {
            Success,
            Failed,
            Canceled
        }
    }
}