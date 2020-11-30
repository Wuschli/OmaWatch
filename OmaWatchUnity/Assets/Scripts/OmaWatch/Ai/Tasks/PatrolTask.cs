using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.OmaWatch.Ai.Commands;
using Assets.Scripts.OmaWatch.Ai.Tasks.PatrolAction;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai.Tasks
{
    public class PatrolTask : AbstractWorldTask
    {
        private CancellationTokenSource _cancellationTokenSource;
        private List<Transform> _patrolPoints;

        private readonly List<Component> _componentsList = new List<Component>();

        private void Awake()
        {
            _patrolPoints = new List<Transform>();
            foreach (var child in transform)
                _patrolPoints.Add((Transform)child);
        }

        public override async Task<TaskResult> Run(AgentBehaviour agent)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                foreach (var patrolPoint in _patrolPoints)
                {
                    await agent.ExecuteCommand(new MoveToPositionCommand(patrolPoint, _cancellationTokenSource.Token));
                    patrolPoint.GetComponents(_componentsList);
                    foreach (var patrolAction in _componentsList.OfType<IPatrolAction>())
                        await patrolAction.Execute(agent, _cancellationTokenSource.Token);
                }

                return TaskResult.Success;
            }
            catch (OperationCanceledException)
            {
                return TaskResult.Cancelled;
            }
        }

        public override void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}