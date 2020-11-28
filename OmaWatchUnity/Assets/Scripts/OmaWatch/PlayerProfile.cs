using System;
using MessagePack;

namespace Assets.Scripts.OmaWatch
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class PlayerProfile
    {
        public Guid Id { get; set; }
        public bool SkipTutorial { get; set; }
    }
}