using UnityEngine;

namespace Assets.Scripts.OmaWatch
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsPlayerController : AbstractPlayerController
    {
        public Rigidbody2D Rigidbody;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected override void FixedUpdate()
        {
            if (Time.timeScale == 0)
                return;

            base.FixedUpdate();
            var velocity = MoveInput * (Time.fixedDeltaTime * Speed);
            velocity *= new Vector2(1f, 0.5f);
            Rigidbody.MovePosition(Rigidbody.position + velocity);
        }
    }
}