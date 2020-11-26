using System;
using Assets.Scripts.OmaWatch;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LeaderboardEntry : MonoBehaviour
    {
        public TMP_Text Position;
        public TMP_Text DisplayName;
        public TMP_Text Score;
        public Color HighlightColor;
        private PlayerLeaderboardEntry _entry;

        public PlayerLeaderboardEntry Entry
        {
            get => _entry;
            set
            {
                _entry = value;
                Present(_entry);
            }
        }

        private void Present(PlayerLeaderboardEntry entry)
        {
            Position.text = $"#{entry.Position + 1}";
            if (!string.IsNullOrEmpty(entry.DisplayName))
                DisplayName.text = entry.DisplayName;
            else
                DisplayName.text = entry.PlayFabId;

            Score.text = entry.StatValue.ToScoreDisplay();

            if (entry.PlayFabId == PlayFabManager.Instance.LocalPlayFabId)
            {
                Position.color = HighlightColor;
                DisplayName.color = HighlightColor;
                Score.color = HighlightColor;
            }
        }
    }
}