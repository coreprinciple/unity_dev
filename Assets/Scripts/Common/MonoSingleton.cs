using UnityEngine;

namespace Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance() { return _instance; }

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(_instance);
                Debug.LogError($"Has Duplicated Singlton::{_instance.GetType()}");
            }
            _instance = GetComponent<T>();

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}   

