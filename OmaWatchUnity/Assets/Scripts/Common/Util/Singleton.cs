using System;
using UnityEngine;

namespace Assets.Scripts.Common.Util
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static bool _isQuitting;

        public static T Instance
        {
            get
            {
                if (_instance == null && !_isQuitting)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            var t = GetComponent<T>();
            if (_instance == t)
                return;
            if (_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = t;
            DontDestroyOnLoad(this);
        }

        protected void OnApplicationQuit()
        {
            _isQuitting = true;
        }
    }
}