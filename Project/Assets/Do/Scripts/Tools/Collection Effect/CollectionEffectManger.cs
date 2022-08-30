using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Do.Scripts.Tools.Collection_Effect
{
    public class CollectionEffectManger : MonoBehaviour
    {
        public static CollectionEffectManger Instance { get; private set; }
        
        public float speed;
        [Range(0.5f, 2f)] [SerializeField] private float range = 1f;
        [SerializeField] private CollectionEffect prefabCollectionEffect;
        [SerializeField] private List<CollectionEffect> listEffectCoin;

        private CollectionEffect _collectionEffect;

        private void Awake()
        {
            if (Instance != null)
                return;
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        public void PlayEffectCoin(Vector2 origin, Vector2 target, Callback callbackStep, Callback complete = null)
        {
            _collectionEffect = GetEffect();
            var tf = _collectionEffect.transform;
            tf.position = origin;
            var between = GetBetween(origin, tf);
            _collectionEffect.ShowEffectCoin(origin, target, between, callbackStep, complete);
        }

        private CollectionEffect GetEffect()
        {
            foreach (var effectCoin in listEffectCoin)
            {
                if (!effectCoin.IsActive)
                    return effectCoin;
            }

            var newEffectCoin = Instantiate(prefabCollectionEffect, transform);
            newEffectCoin.gameObject.name = "Effect " + newEffectCoin.name;
            newEffectCoin.transform.SetSiblingIndex(listEffectCoin.Count + 1);
            listEffectCoin.Add(newEffectCoin);
            return newEffectCoin;
        }

        private List<Vector2> GetBetween(Vector2 origin, Transform tf)
        {
            var between = new List<Vector2>();
            var minX = origin.x - range / 2;
            var maxX = origin.x + range / 2;
            var minY = origin.y - range / 2;
            var maxY = origin.y - range;
            var count = tf.childCount;
            for (var i = 0; i < count; i++)
            {
                between.Add(new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY)));
            }
            return between;
        }
    }
}
