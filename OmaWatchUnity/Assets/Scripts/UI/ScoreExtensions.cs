using System;

namespace Assets.Scripts.UI
{
    public static class ScoreExtensions
    {
        public static string ToScoreDisplay(this int score)
        {
            var timeSpan = TimeSpan.FromMilliseconds(-score);
            return $"{(int) timeSpan.TotalMinutes}:{timeSpan.Seconds:D2}.{timeSpan.Milliseconds:000}";
        }
    }
}