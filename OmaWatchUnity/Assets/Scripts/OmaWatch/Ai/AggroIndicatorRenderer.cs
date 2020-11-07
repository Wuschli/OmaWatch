using System;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.Ai
{
    public class AggroIndicatorRenderer : MonoBehaviour
    {
        public SensorModule sensor;
        public SpriteRenderer renderer;


        private void Update()
        {
            if (sensor.Aggro > 0)
            {
                if (!renderer.enabled)
                    renderer.enabled = true;

                renderer.color = Color.Lerp(Color.yellow, Color.red, sensor.Aggro);
            }
            else
            {
                if (renderer.enabled)
                    renderer.enabled = false;
            }
        }
    }
}