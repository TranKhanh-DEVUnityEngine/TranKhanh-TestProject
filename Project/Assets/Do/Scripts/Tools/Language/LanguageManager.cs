using System;
using Do.Scripts.Tools.Other;
using UnityEngine;

namespace Do.Scripts.Tools.Language
{
    public class LanguageManager : MonoBehaviour
    {
        private const string KEY_LANGUAGE = "KEY_LANGUAGE";
        
        public Language Language { get; private set; }

        private string _english, _vietnamese;

        public static LanguageManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                return;
            Instance = this;
            // DontDestroyOnLoad(gameObject);
            InitLanguage();
        }

        private void InitLanguage()
        {
            _english = SystemLanguage.English.ToString();
            _vietnamese = SystemLanguage.Vietnamese.ToString();
            var saveLanguage = MyPref.GetInt(KEY_LANGUAGE);
            if (saveLanguage == 0)
            {
                if (Application.systemLanguage == SystemLanguage.Vietnamese)
                {
                    MyPref.SetInt(KEY_LANGUAGE, 2);
                    Language = Language.Vie;
                    SetLanguageLocalize(_vietnamese);
                }
                else
                {
                    MyPref.SetInt(KEY_LANGUAGE, 1);
                    Language = Language.Eng;
                    SetLanguageLocalize(_english);
                }
            }
            else if (saveLanguage == 1)
            {
                Language = Language.Eng;
                SetLanguageLocalize(_english);
            }
            else if (saveLanguage == 2)
            {
                Language = Language.Vie;
                SetLanguageLocalize(_vietnamese);
            }
        }

        public void SetLanguage_English()
        {
            if (Language == Language.Eng)
                return;
            MyPref.SetInt(KEY_LANGUAGE, 1);
            SetLanguageLocalize(_english);
            Language = Language.Eng;
        }

        public void SetLanguage_Vietnamese()
        {
            if (Language == Language.Vie)
                return;
            MyPref.SetInt(KEY_LANGUAGE, 2);
            SetLanguageLocalize(_vietnamese);
            Language = Language.Vie;
        }

        private void SetLanguageLocalize(string language)
        {
            Debug.Log("Language Localize : " + language);
            // LocalizationManager.CurrentLanguage = language;
        }
    }
    public enum Language
    {
        Eng,
        Vie
    }
}
