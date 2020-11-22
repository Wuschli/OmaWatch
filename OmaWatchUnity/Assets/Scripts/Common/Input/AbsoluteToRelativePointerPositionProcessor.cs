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
    public class AbsoluteToRelativePointerPositionProcessor : InputProcessor<Vector2>
    {
#if UNITY_EDITOR
        static AbsoluteToRelativePointerPositionProcessor()
        {
            Initialize();
        }
#endif

        public override Vector2 Process(Vector2 value, InputControl control)
        {
            var result = new Vector2(value.x / Screen.width, value.y / Screen.height);
            //Debug.Log(result);
            return result;
        }

        [RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            InputSystem.RegisterProcessor<AbsoluteToRelativePointerPositionProcessor>();
        }
    }
}