
using UnityEngine;

namespace Do.Scripts.Tools.Other
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = GetComponent<T>();
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}
