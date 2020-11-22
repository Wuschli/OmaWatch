using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Assets.Scripts.Common.Input
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class OffsetVector2Processor : InputProcessor<Vector2>
    {
        [Tooltip("Offset value to shift the incoming Vector2's X component by.")]
        public float X = 1;

        [Tooltip("Offset value to shift the incoming Vector2's Y component by.")]
        public float Y = 1;

#if UNITY_EDITOR
        static OffsetVector2Processor()
        {
            Initialize();
        }
#endif

        public override Vector2 Process(Vector2 value, InputControl control)
        {
            return value + new Vector2(X, Y);
        }

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            InputSystem.RegisterProcessor<OffsetVector2Processor>();
        }
    }
}