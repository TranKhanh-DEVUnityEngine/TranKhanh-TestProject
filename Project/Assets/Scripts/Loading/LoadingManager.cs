using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
// using DG.Tweening;
using Do.Scripts.Tools.Language;
using Do.Scripts.Tools.Other;
using Do.Scripts.Tools.Tween;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// using UnityEngine.UI;

namespace Script.Loading
{
    public class LoadingManager : MonoBehaviour
    {
        
        #region Variable
        
        private static LoadingManager instance;
        
        public const string SCENE_LOADING = "LoadingScene";
        public const string SCENE_DATA = "DataScene";
        public const string SCENE_HOME = "UI";
        public const string SCENE_GAME = "INGAME";
        
        public static LoadingManager Instance
        {
            get
            {
                if (instance != null) 
                    return instance;
                var transform = Instantiate(Resources.Load<Transform>("LoadingManager"));
                DontDestroyOnLoad(transform.gameObject);
                instance = transform.GetComponent<LoadingManager>();
                return instance;
            }
        }

        private const float DelayTime = 0.2f;
        private const float Accelerate = 0.5f;
        private const float EndDelayTime = 0.3f;
        [Header("UI")]
        [SerializeField]
        private GameObject root;
        [SerializeField]
        private Image sliderFill;
        [SerializeField] private TextMeshProUGUI text;
        private int _textCount;
        private string _content;

        private AsyncOperation _async;
        private bool _loadScene;
        private bool _loadSceneDone, _loadFillDone;
        private IEnumerator _routineScene, _routineTitle;
        private Callback _callbackLoadRoutine, _callBackLoadUpdate;
        private readonly List<Callback> _listLoadData = new List<Callback>();
        private readonly List<Callback> _listEndLoad = new List<Callback>();
        private bool _isLoadingScene;

        [SerializeField] private GameObject resource;

        #endregion

        public bool IsGamePlay => SceneManager.GetActiveScene().name == SCENE_GAME;

        private void Awake()
        {
            if (instance != null) 
                return;
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (SceneManager.GetActiveScene().name == SCENE_LOADING)
                _isLoadingScene = true;
        }

        private void Start()
        {
            if (LanguageManager.Instance == null)
                Instantiate(resource);
            Invoke(nameof(Init), 1);
        }

        private void Init()
        {
            if (!_isLoadingScene)
            {
                PlayAnimationOnce();
            }
        }

        private void CheckBanner()
        {
            // set loading fill order banner
        }

        #region LoadSceneStep
        private IEnumerator LoadAsyncScene(string nameScene)
        {
            _loadScene = true;
            _loadSceneDone = false;
            _async = new AsyncOperation();
            _async = SceneManager.LoadSceneAsync(nameScene);
            _async.allowSceneActivation = false;
            while (!_loadSceneDone)
            {
                yield return null;
                if (_async.progress < 0.9f) 
                    continue;
                _loadSceneDone = true;
                if (_loadFillDone)
                {
                  //  Debug.Log("Loading Async Late");
                    _async.allowSceneActivation = true;
                    _loadScene = false;
                    _callbackLoadRoutine?.Invoke();
                }
                else 
                    Debug.Log("Loading Async Soon");
            }

        }

        private void LoadSceneWithFill(float target, Callback callback)
        {
            _loadFillDone = false;
            var time = target * Accelerate;
            // MyTween.Instance.MyTween_Float(time, DelayTime, delegate
            // {
            //     if (_loadScene)
            //     {
            //         _loadFillDone = true;
            //         if (_loadSceneDone)
            //         {
            //             Debug.Log("Loading Scene End Fill");
            //             _async.allowSceneActivation = true;
            //             _loadScene = false;
            //             callback?.Invoke();
            //         }
            //         else 
            //             Debug.Log("None Scene End Fill");
            //     }
            //     else
            //     {
            //         Debug.Log("Loading None Load Scene");
            //         callback?.Invoke();
            //     }
            // });
            sliderFill.DOFillAmount(target, time).SetDelay(DelayTime).OnComplete(delegate 
            {
                if (_loadScene)
                {
                    _loadFillDone = true;
                    if (_loadSceneDone)
                    {
                      //  Debug.Log("Loading Scene End Fill");
                        _async.allowSceneActivation = true;
                        _loadScene = false;
                        callback?.Invoke();
                    }
                    else 
                        Debug.Log("None Scene End Fill");
                }
                else
                {
                   // Debug.Log("Loading None Load Scene");
                    callback?.Invoke();
                }
            });
        }

        private void LoadDataWithFill(float oldTarget, float newTarget, Callback callback)
        {
            var time = (newTarget - oldTarget) * Accelerate;
            // MyTween.Instance.MyTween_Float(time, DelayTime, delegate
            // {
            //     callback?.Invoke();
            //     CallEventLoadData();
            // });
            sliderFill.DOFillAmount(newTarget, time).SetDelay(DelayTime).OnComplete(delegate 
            {
                callback?.Invoke();
                CallEventLoadData();
            });
        }

        private void LoadToEndWithFill(float oldTarget, float newTarget, Callback callback)
        {
            var time = (newTarget - oldTarget) * Accelerate;
            // MyTween.Instance.MyTween_Float(time, DelayTime, delegate
            // {
            //     callback?.Invoke();
            //     CallEventLoadData();
            // });
            sliderFill.DOFillAmount(newTarget, time).SetDelay(DelayTime).OnComplete(delegate 
            {
                callback?.Invoke();
                CallEventLoadData();
            });
        }

        private void LoadEventEndWithFill()
        {
            Debug.LogError("Load Event End With Fill");
            CallEventLoadData();
            MyTween.Instance.MyTween_Float(EndDelayTime, delegate
            {
                CallEventEndLoading();
                root.SetActive(false);
            });
            Debug.LogError("Load Event End With Fill End");
        }

        #endregion
        
        /// <summary>
        /// /////////
        /// </summary>

        #region LoadOnceScene

        public void LoadScene(string nameScene)
        {
            CheckBanner();
            _loadScene = false;
            _async = new AsyncOperation();
            _listLoadData.Clear();
            _listEndLoad.Clear();
            _routineTitle = PlayTitleText();
            StartCoroutine(_routineTitle);
            if (nameScene != "")
            {
                _routineScene = LoadAsyncScene(nameScene);
                StartCoroutine(_routineScene);
            }
            PlayAnimationOnce();
        }

        private void PlayAnimationOnce()
        {
            root.SetActive(true);
            sliderFill.fillAmount = 0;
            var target = Random.Range(0.2f, 0.3f);
            _callbackLoadRoutine = delegate
            {
                var newTarget = Random.Range(0.8f, 0.9f);
                LoadDataWithFill(target, newTarget, delegate
                {
                    LoadToEndWithFill(newTarget, 1f, LoadEventEndWithFill);
                });
            };
            LoadSceneWithFill(target, _callbackLoadRoutine);
        }
        

        #endregion

        /// <summary>
        /// /////////
        /// </summary>

        #region LoadDoubleScene

        public void LoadScene(string scene1, string scene2)
        {
            CheckBanner();
            _loadScene = false;
            _async = new AsyncOperation();
            _listLoadData.Clear();
            _listEndLoad.Clear();
            _routineTitle = PlayTitleText();
            StartCoroutine(_routineTitle);
            if (scene1 != "")
            {
                _routineScene = LoadAsyncScene(scene1);
                StartCoroutine(_routineScene);
            }
            PlayAnimationDouble(scene2);
        }

        private void PlayAnimationDouble(string scene2)
        {
            root.SetActive(true);
            sliderFill.fillAmount = 0;
            var target = Random.Range(0.1f, 0.15f);
            _callbackLoadRoutine = delegate
            {
                var newTarget = Random.Range(0.4f, 0.45f);
                LoadDataWithFill(target, newTarget, delegate
                {
                    target = Random.Range(0.45f, 0.5f);
                    LoadToEndWithFill(newTarget, target, delegate
                    {
                        if (scene2 != "")
                        {
                            _routineScene = LoadAsyncScene(scene2);
                            StartCoroutine(_routineScene);
                        }
                        target = Random.Range(0.55f, 0.6f);
                        _callbackLoadRoutine = delegate
                        {
                            newTarget = Random.Range(0.9f, 0.95f);
                            LoadDataWithFill(target, newTarget, delegate
                            {
                                target = 1f;
                                LoadToEndWithFill(newTarget, target, LoadEventEndWithFill);
                            });
                        };
                        LoadSceneWithFill(target, _callbackLoadRoutine);
                    });
                });
            };
            LoadSceneWithFill(target, _callbackLoadRoutine);
        }

        #endregion
        
        /// <summary>
        /// /////////
        /// </summary>

        #region CallBackEvent

        public void AddCallBackLoadData(Callback callback)
        {
            _listLoadData.Add(callback);
        }

        public void AddCallBackEndLoading(Callback callback)
        {
            _listEndLoad.Add(callback);
        }

        private void CallEventLoadData()
        {
            if (_listLoadData.Count <= 0) 
                return;
//            Debug.Log("Load Data Count : " + _listLoadData.Count);
            foreach (var callback in _listLoadData)
            {
                callback?.Invoke();
//                Debug.Log("Load Data Step");
            }
            _listLoadData.Clear();
        }

        private void CallEventEndLoading()
        {
            CallEventLoadData();
            if (_listEndLoad.Count <= 0)
                return;
//            Debug.Log("Load End Count : " + _listEndLoad.Count);
            foreach (var callback in _listEndLoad)
            {
                callback?.Invoke();
               // Debug.Log("Load End Step");
            }
            _listEndLoad.Clear();
        }

        #endregion

        private IEnumerator PlayTitleText()
        {
            var yield = Yield.GetTime(0.5f * Accelerate);
            _textCount = 0;
            _content = LanguageManager.Instance.Language == Language.Eng ? "Loading" : "Đang tải";
            //while (root.activeSelf)
            while (root.activeSelf)
            {
                switch (_textCount)
                {
                    case 0:
                        text.text = _content + ".";
                        _textCount++;
                        break;
                    case 1:
                        text.text = _content + "..";
                        _textCount++;
                        break;
                    case 2:
                        text.text = _content + "...";
                        _textCount = 0;
                        break;
                }
                yield return yield;
            }
        }
    }
}
