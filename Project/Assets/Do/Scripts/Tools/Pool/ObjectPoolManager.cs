using System.Collections.Generic;
using System.Linq;
using Do.Scripts.Tools.Other;
using UnityEngine;

namespace Do.Scripts.Tools.Pool
{
    public enum PoolId
    {
     Box1,
     Box2,
     Box3

    }
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        private readonly Dictionary<PoolId, ObjectPool> _dictionary = new Dictionary<PoolId, ObjectPool>();
        
        [Header("Controller")]
        [SerializeField] private int subPoolLenght = 10;
        private readonly List<GameObject> _listSubPool = new List<GameObject>();

        [SerializeField] private List<ObjectSource> objectSources = new List<ObjectSource>();

        private void Initialized()
        {
            if (objectSources.Count <= 0)
                return;
            foreach (var objectSource in objectSources)
            {
                InstancePool(objectSource);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Initialized();
        }

        private void InstancePool(ObjectSource source)
        {
            if (_dictionary.ContainsKey(source.id))
            {
                Debug.LogError("Error : Contains Key " + source.id);
                return;
            }
            var go = new GameObject(source.id + " Pools");
            var objectPool = go.AddComponent<ObjectPool>();
            go.transform.parent = gameObject.transform;
            objectPool.Initialize(source);
            _dictionary.Add(source.id, objectPool);
        }

        public GameObject GetPooledObject(PoolId id)
        {
            if (_dictionary.ContainsKey(id)) 
                return _dictionary[id].GetPooledObject();
            Debug.LogError("Cannot Find Pool " + id);
            return null;
        }

        public void DisableAllObject()
        {
            var list = _dictionary.Values.ToList();
            foreach (var objectPool in list.Where(objectPool => objectPool != null))
            {
                objectPool.DisableAllObject();
            }

            if (_listSubPool.Count <= 0)
                return;
            foreach (var pool in _listSubPool.Where(pool => pool != null))
            {
                pool.GetComponent<BasePoolManager>().DisableAllObject();
            }
        }

        public void AddSubPool(GameObject addPool)
        {
            if (_listSubPool.Count > 0)
            {
                var isExist = false;
                foreach (var pool in _listSubPool.Where(pool => addPool == pool))
                {
                    isExist = true;
                }
                if (isExist)
                {
                    _listSubPool.Remove(addPool);
                }
                else
                {
                    if (_listSubPool.Count >= subPoolLenght)
                    {
                        var old = _listSubPool[0];
                        _listSubPool.Remove(old);
                        Destroy(old);
                    }
                }
            }
            _listSubPool.Add(addPool);
        }

        public void RemoveSubPool(GameObject removePool)
        {
            _listSubPool.Remove(removePool);
            Destroy(removePool);
        }
    }
}
