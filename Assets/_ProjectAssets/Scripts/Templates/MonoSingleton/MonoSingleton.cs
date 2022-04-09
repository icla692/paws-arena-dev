using UnityEngine;

namespace Anura.Templates.MonoSingleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = LoadLazyInstance();
                return instance;
            }
            set
            {
                instance = value;
                DontDestroyOnLoad(instance.gameObject);
            }
        }

        private static T instance;

        private void Awake()
        {
            Singleton();
            MonoAwake();
        }

        private void Singleton()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                if (instance != (T)this)
                {
                    Destroy(this);
                }
            }
        }

        private static T LoadLazyInstance()
        {
            Debug.LogWarning(typeof(T) + " is null.");

            var managersParent = GameObject.FindWithTag("Managers");
            var manager = managersParent.GetComponentInChildren<T>();
            if (manager == null)
            {
                manager = CreateInstance(managersParent.transform);
            }

            return manager;
        }

        private static T CreateInstance(Transform newParent)
        {
            Debug.LogError(typeof(T) + " doesn't exist.");

            var instanceGameObject = new GameObject(typeof(T) + " - MonoSingleton", typeof(T));
            instanceGameObject.transform.parent = newParent.transform;
            return instanceGameObject.GetComponent<T>();
        }

        /// <summary>
        /// The method is call in Awake();
        /// </summary>
        protected virtual void MonoAwake()
        {

        }

        protected virtual void OnValidate()
        {
            gameObject.name = typeof(T).Name;
        }
    }
}
