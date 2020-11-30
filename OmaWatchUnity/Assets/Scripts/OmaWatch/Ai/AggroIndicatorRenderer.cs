using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class AggroIndicatorRenderer : MonoBehaviour
    {
        [FormerlySerializedAs("sensor")]
        public SensorModule Sensor;

        [FormerlySerializedAs("renderer")]
        public SpriteRenderer Renderer;

        public SpriteRenderer IncreaseIndicator;

        private void Update()
        {
            if (Sensor.Aggro > 0)
            {
                SetEnabled(Renderer, true);

                Renderer.color = Color.Lerp(Color.yellow, Color.red, Sensor.Aggro);

                if (Sensor.AggroIncreasing)
                    SetEnabled(IncreaseIndicator, true);
                else
                    SetEnabled(IncreaseIndicator, false);
            }
            else
            {
                SetEnabled(Renderer, false);
                SetEnabled(IncreaseIndicator, false);
            }
        }

        private void SetEnabled(SpriteRenderer renderer, bool state)
        {
            if (renderer.enabled != state)
                renderer.enabled = state;
        }
    }
}