using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace Assets.Scripts
{
    public class Startup
    {
        static bool _serializerRegistered = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Initialize()
        {
            if (!_serializerRegistered)
            {
                StaticCompositeResolver.Instance.Register(
                    MessagePack.Resolvers.GeneratedResolver.Instance,
                    MessagePack.Resolvers.StandardResolver.Instance
                );

                var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

                MessagePackSerializer.DefaultOptions = option;
                _serializerRegistered = true;
            }
        }

#if UNITY_EDITOR


        [UnityEditor.InitializeOnLoadMethod]
        static void EditorInitialize()
        {
            Initialize();
        }

#endif
    }
}