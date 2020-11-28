﻿using Assets.Scripts.Common.Util;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SplashController : MonoBehaviour
    {
        protected void OnEnable()
        {
            UIManager.Instance.Fire(UITrigger.SplashDone).FireAndForget();
        }
    }
}