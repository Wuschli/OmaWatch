using System;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractTouchInteraction : AbstractInteraction
    {
        protected virtual void OnEnable()
        {
            var collider2D = GetComponent<Collider2D>();
            if (collider2D == null)
            {
                Debug.LogError("Touch Interaction needs a Collider 2D!", gameObject);
                return;
            }

            collider2D.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerController = other.gameObject.GetComponent<AbstractPlayerController>();
            if (playerController == null)
                return;
            InteractAsync(playerController);
        }
    }
}