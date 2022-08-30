using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Do.Scripts.Tools.Audio;
using Do.Scripts.Tools.Other;
using UnityEngine;

namespace Do.Scripts.Tools.Collection_Effect
{
    public class CollectionEffect : MonoBehaviour
    {
        public new string name;
        [Range(0.02f, 0.2f)] [SerializeField] private float duration = 0.1f;
        [SerializeField] private CollectionEffectManger collectionEffectManger;
        [SerializeField] private List<Animator> animators;

        private List<Vector2> _between = new List<Vector2>();
        private float _count;
        private Vector2 _target;
        private IEnumerator _routine;
        private readonly int _play = Animator.StringToHash("Play");
        private Callback _update, _complete;
        private WaitForSeconds _waitForSeconds;
        public bool IsActive { get; private set; }

        private void Awake()
        {
            _waitForSeconds = Yield.GetTime(duration);
        }

        public void ShowEffectCoin(Vector2 origin, Vector2 target, List<Vector2> between, Callback update, Callback complete = null, int count = 0)
        {
            transform.position = origin;
            _count = count <= 0 ? transform.childCount : count;
            _target = target;
            _between = between;
            _update = update;
            _complete = complete;
            IsActive = true;
            gameObject.SetActive(true);
            _routine = InstanceAllObject();
            StartCoroutine(_routine);
        }

        private IEnumerator InstanceAllObject()
        {
            var current = 0;
            while (current < _count)
            {
//                AudioManager.Instance.Play(isCoin ? SoundType.CoinCollect : SoundType.StarCollect);
                var sort = _count - current;
                var child = transform.GetChild(current);
                child.gameObject.SetActive(true);
                    Move(child, (SoundType) 0, sort <= 1);

                if (animators.Count > 0)
                {
                    animators[current].SetTrigger(_play);
                    animators[current].speed = 1 / collectionEffectManger.speed;
                }
                current++;
                if (current < _count)
                    yield return _waitForSeconds;
            }
        }

        private void Move(Transform child, Vector2 between, SoundType soundType, bool isLast)
        {
            var speed = collectionEffectManger.speed;
            speed -= 0.2f;
            child.DOMove(between, speed / 2).SetEase(Ease.OutSine).OnComplete(delegate
            {
                child.DOMove(_target, speed).SetEase(Ease.InSine).OnComplete(delegate
                {
                    AudioManager.Instance.Play(soundType);
                    _update?.Invoke();
                    if (isLast)
                        DisableAllObject();
                });
            });
        }

        private void Move(Transform child, SoundType soundType, bool isLast)
        {
            child.DOMove(_target, collectionEffectManger.speed).SetEase(Ease.InSine).OnComplete(delegate
            {
                AudioManager.Instance.Play(soundType);
                _update?.Invoke();
                if (isLast)
                {
                    DisableAllObject();
                }
            });
        }

        private void DisableAllObject()
        {
            gameObject.SetActive(false);
            for (var i = 0; i < _count; i++)
            {
                var child = transform.GetChild(i);
                child.localPosition = Vector2.zero;
                child.gameObject.SetActive(false);
            }
            IsActive = false;
            _routine = null;
        }
    }
}
