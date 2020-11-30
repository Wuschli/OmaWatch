using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    public abstract class AbstractButtonInteraction : AbstractInteraction
    {
        public AbstractInputIconView InputIconView;

        protected virtual void OnEnable()
        {
            var collider2D = GetComponent<Collider2D>();
            if (collider2D == null)
            {
                Debug.LogError("Button Interaction needs a Collider 2D!", gameObject);
                return;
            }

            collider2D.isTrigger = true;

            if (InputIconView == null)
                InputIconView = GetComponentInChildren<AbstractInputIconView>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var playerController = other.gameObject.GetComponent<AbstractPlayerController>();
            if (playerController == null)
                return;
            playerController.AddPossibleInteraction(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var playerController = other.gameObject.GetComponent<AbstractPlayerController>();
            if (playerController == null)
                return;
            playerController.RemovePossibleInteraction(this);
        }
    }
}