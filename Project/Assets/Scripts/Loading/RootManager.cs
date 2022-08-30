
using Do.Scripts.Tools.Audio;
using Do.Scripts.Tools.Other;
using Do.Scripts.Tools.Pool;
using UnityEngine;

namespace Script.Loading
{
    public class RootManager : Singleton<RootManager>
    {
        #region Bool

        public bool isTestAds;
        public bool isTestInApp;
        public bool isTestNotifile;
        private const string REMOVE_ADS = "REMOVE_ADS";
        private const string RATE = "RATE";
        
        private bool _isRemoveAds, _isRate;
        public bool IsRemoveAds
        {
            get => _isRemoveAds;
            set
            {
                _isRemoveAds = value;
                MyPref.SetBool(REMOVE_ADS, _isRemoveAds);
                //iiiiiiii
            }
        }

        public bool IsRate
        {
            get => _isRate;
            set
            {
                _isRate = value;
                MyPref.SetBool(RATE, _isRate);
            }
        }

        #endregion
        protected override void Awake()
        {
            base.Awake();
            IsRemoveAds = MyPref.GetBool(REMOVE_ADS);
            IsRate = MyPref.GetBool(RATE);
        }
        
        public void PlayGame(bool stopMusic = true)
        {
            LoadingManager.Instance.LoadScene(LoadingManager.SCENE_GAME);
            if (stopMusic)
                AudioManager.Instance.StopMusic();
        }

        public void BackHome()
        {
            LoadingManager.Instance.LoadScene(LoadingManager.SCENE_HOME);
            AudioManager.Instance.StopAll();
            ObjectPoolManager.Instance.DisableAllObject();
            // if (HandleFireBase.Instance != null)
            // {
            //     HandleFireBase.Instance.LogEventWithString(HandleFireBase.OPEN_HOME);
            // }
            // if (AdsManager.Instance != null)
            // {
            //     AdsManager.Instance.showIntertital(true,false,false,null);
            // }
        }
        
        public void OpenLinkRate()
        {
#if UNITY_ANDROID
            Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier);
#elif UNITY_IOS
     //   Application.OpenURL("http://www.itunes.com/app/1498751007");
#endif
        }
    }
}
