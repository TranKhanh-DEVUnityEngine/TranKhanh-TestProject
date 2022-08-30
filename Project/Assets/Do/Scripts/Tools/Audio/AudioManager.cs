using System;
using System.Collections.Generic;
using System.Linq;
using Do.Scripts.Tools.Other;
using UnityEngine;

namespace Do.Scripts.Tools.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {

        #region Source
        
        [Header("Source")]
        [SerializeField] private List<AudioMusic> musicClipSource;
        [SerializeField] private List<AudioSound> soundClipSource;
        [SerializeField] private AudioSourcePool audioSourcePoolPrefab;
        
        #endregion
        
        /// <summary>
        /// /////////
        /// </summary>
        
        #region Variable
        
        [Header("Variable")]
        [SerializeField] private AudioSourcePool musicSource;
        [SerializeField] private AudioSourcePool soundSourceCache;
        [SerializeField] private List<AudioSourcePool> listSoundSourcePools;
        private readonly Dictionary<MusicType, AudioClip> _dictionaryMusic = new Dictionary<MusicType, AudioClip>();
        private readonly Dictionary<SoundType, AudioClip> _dictionarySound = new Dictionary<SoundType, AudioClip>();
        private readonly Dictionary<SoundType, float> _dictionarySoundDuration = new Dictionary<SoundType, float>();
        private readonly Dictionary<SoundType, float> _dictionarySoundTimes = new Dictionary<SoundType, float>();

        #endregion
        
        /// <summary>
        /// /////////
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            InitBoolean();
            InitSource();
            SetMusicMute();
            SetSoundMute();
        }

        #region Bool

        private const string KEY_SOUND = "KEY_SOUND";
        private const string KEY_MUSIC = "KEY_MUSIC";
        private const string KEY_VIBRATION = "KEY_VIBRATION";
        
        private bool _sound, _music, _vibration;

        public bool Sound
        {
            get => _sound;
            set
            {
                _sound = value;
                MyPref.SetBool(KEY_SOUND, _sound);
                SetSoundMute();
            }
        }

        public bool Music
        {
            get => _music;
            set
            {
                _music = value;
                MyPref.SetBool(KEY_MUSIC, _music);
                SetMusicMute();
            }
        }

        public bool Vibration
        {
            get => _vibration;
            set
            {
                _vibration = value;
                MyPref.SetBool(KEY_VIBRATION, _vibration);
            }
        }

        private void InitBoolean()
        {
            Sound = MyPref.GetBool(KEY_SOUND, true);
            Music = MyPref.GetBool(KEY_MUSIC, true);
            Vibration = MyPref.GetBool(KEY_VIBRATION, true);
        }

        #endregion

        private void InitSource()
        {
            foreach (var music in musicClipSource)
            {
                _dictionaryMusic.Add(music.musicType, music.audioClip);
            }
            foreach (var sound in soundClipSource)
            {
                _dictionarySound.Add(sound.soundType, sound.audioClip);
                _dictionarySoundDuration.Add(sound.soundType, sound.duration);
                _dictionarySoundTimes.Add(sound.soundType, 0f);
            }
        }

        private void Update()
        {
            var deltaTime = Time.unscaledDeltaTime;
            var listSoundTime = _dictionarySoundTimes.ToList();
            foreach (var item in listSoundTime)
            {
                var time = item.Value;
                if (time <= 0)
                    continue;
                time -= deltaTime;
                if (time < 0)
                    time = 0;
                _dictionarySoundTimes[item.Key] = time;
            }
        }

        public void SetMusicMute()
        {
            musicSource.UnMute = Music;
        }

        public void SetSoundMute()
        {
            soundSourceCache.UnMute = Sound;
            foreach (var soundSource in listSoundSourcePools)
            {
                soundSource.UnMute = Sound;
            }
        }

        public void Play(MusicType musicType)
        {
            musicSource.Play(_dictionaryMusic[musicType]);
        }

        public void Play(SoundType soundType)
        {
            if (_dictionarySoundTimes[soundType] > 0)
                return;
            GetAudioSourceInSourcePool().PlayOneShot(_dictionarySound[soundType]);
            _dictionarySoundTimes[soundType] = _dictionarySoundDuration[soundType];
        }

        public void PlayCache(SoundType soundType)
        {
            if (soundSourceCache.IsPlaying)
                soundSourceCache.Stop(true);
            soundSourceCache.PlayOneShot(_dictionarySound[soundType]);
        }

        public void Play()
        {
            if (Vibration)
                global::Vibration.Vibrate();
        }

        public void Play(long millisecond)
        {
            if (Vibration)
                global::Vibration.Vibrate(millisecond);
        }

        public void StopMusic()
        {
            if (musicSource.IsPlaying)
                musicSource.Stop();
        }

        public void StopSound()
        {
            soundSourceCache.Stop();
            foreach (var soundSource in listSoundSourcePools.Where(soundSource => soundSource.IsPlaying))
            {
                soundSource.Stop();
            }
        }

        public void StopAll()
        {
            StopMusic();
            StopSound();
        }

        private AudioSourcePool GetAudioSourceInSourcePool()
        {
            foreach (var audioSource in listSoundSourcePools)
            {
                if (audioSource.IsPlaying) 
                    continue;
                return audioSource;
            }
            var audioSourcePool = Instantiate(audioSourcePoolPrefab, transform);
            audioSourcePool.UnMute = Sound;
            listSoundSourcePools.Add(audioSourcePool);
            return audioSourcePool;
        }
    }

    [Serializable]
    public class AudioMusic
    {
        public MusicType musicType;
        public AudioClip audioClip;
    }

    [Serializable]
    public class AudioSound
    {
        public SoundType soundType;
        public AudioClip audioClip;
        public float duration = 0.1f;
    }

    public enum MusicType
    {
        Home,
        InGame
    }

    public enum SoundType
    {
        Click,
        CoinCollect,
        WeaponShow,
        FishCollectionSnap,
        Upgrade,
        UpgradeIsland,
        Combo,
        CompleteGame,
        CatInCollection,
        CatJump,
        LandingShip,
        CatDive,
        CatWarningOxy,
        NormalShoot,
        ChensShoot,
        RailyShoot,
        RazorShoot,
        StrokeTitanShoot,
        TinyPowerUp,
        BoosterBaitShoot,
        BoosterFishnetShoot,
        BoosterBaitAction,
        BoosterIceAction,
        FishButterflyEat,
        FishBlock,
        FishRayThunder,
        CatlingGun,
        Chens,
        Pretador,
        Sawgun,
        Bait,
        TrippleHook,
        Raily,
        StrokeTitan,
        Sven
        
    }
}
