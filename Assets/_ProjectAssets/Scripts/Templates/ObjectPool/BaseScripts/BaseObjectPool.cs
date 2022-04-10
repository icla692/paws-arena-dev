using System.Collections.Generic;
using Anura.Templates.ObjectPool.Interfaces;
using UnityEngine;

namespace Anura.Templates.ObjectPool.BaseScripts
{
    public abstract class BaseObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : Component
    {
        protected readonly Queue<T> pool = new Queue<T>();

        public virtual T GetObjectFromPool(bool isActive = true)
        {
            LazyInstantiation();
            var element = pool.Dequeue();
            element.gameObject.SetActive(isActive);
            return element;
        }

        public virtual void AddObjectToPool(T component)
        {
            EnqueueComponent(component);
        }

        protected abstract T GetComponent();

        private void LazyInstantiation()
        {
            if (pool.Count > 0)
                return;

            EnqueueComponent(Instantiate(GetComponent()));
        }

        private void EnqueueComponent(T component)
        {
            component.gameObject.SetActive(false);
            if (!pool.Contains(component))
            {
                pool.Enqueue(component);
            }
            else
            {
                Debug.LogError("The pool contains the component " + component.name +"/nID: "+ component.GetInstanceID());
            }
        }
    }
}
