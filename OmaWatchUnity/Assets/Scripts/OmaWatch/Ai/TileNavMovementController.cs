using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.World;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class TileNavMovementController : MonoBehaviour
    {
        [Range(0.1f, 5f)]
        public float Speed = 1f;

        public Transform DebugTargetPos;
        public GameObject TileIndicator;
        public GameObject NextMovePosIndicator;
        public Animator Animator;

        private TaskCompletionSource<MoveResult> _currentOp;
        private CancellationToken? _currentCancellationToken;
        private readonly Queue<Vector3> _movePositions = new Queue<Vector3>();


        private Vector3 _prevPos;
        private Transform _currentTarget;
        private Vector3Int _currentTargetTile;
        private Vector3 _lastDirection;
        private Vector3 _lastPosition;


        private void Start()
        {
            if (DebugTargetPos != null)
                MoveToTarget(DebugTargetPos, CancellationToken.None);
        }

        protected void OnEnable()
        {
            if (Animator == null)
                Animator = GetComponent<Animator>();
            _lastPosition = transform.position;
            _lastDirection = Vector3.zero;
        }

        public Task<MoveResult> MoveToTarget(Transform target, CancellationToken cancellationToken)
        {
            if (_currentOp != null)
                CompleteOp(MoveResult.Canceled);

            _currentTarget = target;
            _currentCancellationToken = cancellationToken;

            var currentTargetPosition = target.position;
            _currentTargetTile = WorldRoot.Instance.GetTilePos(currentTargetPosition);
            var path = WorldRoot.Instance.GetPath(transform.position, currentTargetPosition);
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

            var travelDistance = Speed * Time.deltaTime;
            var distanceToTarget = Vector3.Distance(transform.position, _movePositions.Peek());
            if (travelDistance < distanceToTarget)
                transform.position = Vector3.MoveTowards(transform.position, _movePositions.Peek(), travelDistance);
            else
            {
                transform.position = _movePositions.Dequeue();

                //target has moved, re-calc the path!
                var tileNow = WorldRoot.Instance.GetTilePos(_currentTarget.position);
                if (tileNow != _currentTargetTile)
                {
                    _currentTargetTile = tileNow;
                    var path = WorldRoot.Instance.GetPath(transform.position, _currentTarget.position);
                    if (path != null)
                    {
                        _movePositions.Clear();
                        path.ForEach(p => _movePositions.Enqueue(p));
                    }
                    else
                    {
                        Debug.LogWarning("Target path was not found.");
                    }
                }
            }

            if (_movePositions.Count == 0)
                CompleteOp(MoveResult.Success);
        }

        protected void LateUpdate()
        {
            var traveledVector = (transform.position - _lastPosition) / Speed / Time.deltaTime;
            if (traveledVector.sqrMagnitude > 1)
                traveledVector.Normalize();
            if (traveledVector.sqrMagnitude > .01f)
                _lastDirection = traveledVector.normalized;
            _lastPosition = transform.position;

            Debug.Log(traveledVector);

            Animator.SetFloat("AbsoluteSpeed", traveledVector.magnitude);
            Animator.SetFloat("Horizontal", _lastDirection.x);
            Animator.SetFloat("Vertical", _lastDirection.y);
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