using System.Collections.Generic;
using UnityEngine;

namespace Do.Scripts.Tools.Pool
{
    public class BasePoolManager : MonoBehaviour
    {
        public float timeDelay = 600;
        private readonly List<ObjectPool> _objectPools = new List<ObjectPool>();
    
        public void LoadData()
        {
            ObjectPoolManager.Instance.AddSubPool(gameObject);
            if(IsInvoking(nameof(DestroyGameObject)))
                CancelInvoke(nameof(DestroyGameObject));
            Invoke(nameof(DestroyGameObject), timeDelay);
        }

        private void DestroyGameObject()
        {
            ObjectPoolManager.Instance.RemoveSubPool(gameObject);
        }

        public void DisableAllObject()
        {
            foreach (var objectPool in _objectPools)
            {
                objectPool.DisableAllObject();
            }
        }

        protected ObjectPool InstancePool(ObjectSource source)
        {
            if (source == null) 
                return null;
            var go = new GameObject(source.prefab.name + " Pools");
            var objectPool = go.AddComponent<ObjectPool>();
            go.transform.parent = gameObject.transform;
            objectPool.Initialize(source);
            _objectPools.Add(objectPool);
            return objectPool;
        }
    }
}