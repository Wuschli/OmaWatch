using System;
using UnityEngine;

namespace Assets.Scripts.OmaWatch.GamePlay.Interactions
{
    [CreateAssetMenu(menuName = "Create RocketConfig", fileName = "RocketConfig", order = 0)]
    public class RocketConfig : ScriptableObject
    {
        public RocketConfigElement[] Elements;

        [Serializable]
        public class RocketConfigElement
        {
            [SerializeField]
            public RocketPartType Type;

            [SerializeField]
            public Anchor Anchor;

            [SerializeField]
            public RocketConfigAttachment[] Attachments;
        }

        [Serializable]
        public class RocketConfigAttachment
        {
            [SerializeField]
            public RocketPartType Type;

            [SerializeField]
            public Anchor Anchor;
        }

        public enum Anchor
        {
            Bottom,
            Center,
            Top
        }
    }
}