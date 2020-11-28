using Assets.Scripts.Common.Util;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.OmaWatch
{
    public class PlayerController : AbstractPlayerController
    {
        protected override void Awake()
        {
            base.Awake();

            var nav = GetComponent<NavMeshAgent>();
            if (nav != null)
            {
                nav.updateRotation = false;
                nav.updateUpAxis = false;
            }
        }

        protected override void Update()
        {
            if (Time.timeScale == 0)
                return;
            base.Update();

            var translation = MoveInput * (Speed * Time.deltaTime);
            translation *= new Vector2(1f, 0.5f);
            transform.Translate(translation.ToVector3());
        }
    }
}