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


        private void Update()
        {
            if (Sensor.Aggro > 0)
            {
                if (!Renderer.enabled)
                    Renderer.enabled = true;

                Renderer.color = Color.Lerp(Color.yellow, Color.red, Sensor.Aggro);
            }
            else
            {
                if (Renderer.enabled)
                    Renderer.enabled = false;
            }
        }
    }
}