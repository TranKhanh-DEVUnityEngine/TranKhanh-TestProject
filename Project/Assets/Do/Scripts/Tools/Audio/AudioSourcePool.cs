using System.Collections;
using DG.Tweening;
using Do.Scripts.Tools.Tween;
using UnityEngine;

namespace Do.Scripts.Tools.Audio
{
    public class AudioSourcePool : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [Range(0f, 1f)] [SerializeField] private float maxVolume = 1f;
        private const float FadeTime = 0.5f;
        public bool IsPlaying { get; private set; }

        public bool UnMute
        {
            set => audioSource.mute = !value;
        }

        private DG.Tweening.Tween _tween;

        private void Awake()
        {
            audioSource.clip = null;
            audioSource.volume = 1;
        }

        public void Play(AudioClip audioClip)
        {
            gameObject.SetActive(true);
            if (IsPlaying)
                return;
            IsPlaying = true;
            if (_tween != null)
            {
                _tween?.Kill();
                _tween = null;
            }
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
            if (audioSource.mute)
                return;
            audioSource.volume = 0;
            _tween = audioSource.DOFade(maxVolume, FadeTime).OnComplete(delegate
            {
                _tween?.Kill();
                _tween = null;
            });
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            gameObject.SetActive(true);
            IsPlaying = true;
            audioSource.volume = maxVolume;
            audioSource.PlayOneShot(audioClip);
            var time = audioClip.length;
            _tween = MyTween.Instance.DoTween_Float(time, 0, delegate
            {
                IsPlaying = false;
                gameObject.SetActive(false);
            });
        }

        public void Stop(bool isSkip = false)
        {
            if (!IsPlaying)
                return;
            IsPlaying = false;
            if (_tween != null)
            {
                _tween?.Kill();
                _tween = null;
            }
            if (audioSource.mute || isSkip)
            {
                audioSource.Stop();
                audioSource.clip = null;
                gameObject.SetActive(false);
            }
            else
            {
                audioSource.volume = maxVolume;
                _tween = audioSource.DOFade(0f, FadeTime).OnComplete(delegate
                {
                    audioSource.Stop();
                    audioSource.clip = null;
                    _tween?.Kill();
                    _tween = null;
                    gameObject.SetActive(false);
                });
            }
        }
    }
}
