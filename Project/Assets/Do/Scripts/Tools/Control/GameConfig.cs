using Do.Scripts.Tools.Other;
using Script.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Do.Scripts.Tools.Control
{
    public class GameConfig : Singleton<GameConfig>
    {
        private enum ScreenType
        {
            Horizontal,
            Vertical
        }
        [SerializeField] private ScreenType screenType;
        
        private Vector2 screenSize = Vector2.zero;
        private float ratioScaleScreen = 0f;

        public Vector2 ScreenSize
        {
            get
            {
                if (screenSize.x > 0)
                    return screenSize;
                screenSize.x = Screen.width;
                screenSize.y = Screen.height;
                return screenSize;
            }
            private set => screenSize = value;
        }

        public float RatioScaleScreen
        {
            get
            {
                if (ratioScaleScreen > 0) 
                    return ratioScaleScreen;
                ratioScaleScreen = GetRatio();
                return ratioScaleScreen;
            }
            private set => ratioScaleScreen = value;
        }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        private void Start()
        {
            if (SceneManager.GetActiveScene().name == LoadingManager.SCENE_LOADING)
            {
                LoadingManager.Instance.LoadScene(LoadingManager.SCENE_DATA, LoadingManager.SCENE_HOME);
            }
        }

        private void Initialize()
        {
            RatioScaleScreen = GetRatio();
//            Debug.Log("Screen Size -- " + ScreenSize + " -- " + RatioScaleScreen);
#if UNITY_EDITOR
            Application.targetFrameRate = -1;
          //  Debug.Log("Editor");
            return;
#endif
#if UNITY_ANDROID
            Application.targetFrameRate = 60;
            Debug.Log("Android");
#endif
        }

        private float GetRatio()
        {
            if (screenType == ScreenType.Vertical)
            {
                var deviceRatio = ScreenSize.x / ScreenSize.y;
                var deviceWidth = 1080 / deviceRatio;
                return deviceWidth / 1920;
            }
            else
            {
                var deviceRatio = ScreenSize.x / ScreenSize.y;
                var deviceHeight = 1920 / deviceRatio;
                return deviceHeight / 1080;
            }
        }
    }
}
