using UnityEngine;

namespace MyDemo
{
    public class Sigleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                return instance;
            }
        }
        protected virtual void Awake()
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}

