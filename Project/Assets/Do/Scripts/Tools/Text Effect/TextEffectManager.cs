using System;
using System.Collections.Generic;
using UnityEngine;

namespace Do.Scripts.Tools.Text_Effect
{
    public class TextEffectManager : MonoBehaviour
    {
        public static TextEffectManager Instance { get; private set; }
        
        public float speed;
        
        [SerializeField] private TextEffect prefab;

        private readonly List<TextEffect> _effectTexts = new List<TextEffect>();

        private void Awake()
        {
            if (Instance != null)
                return;
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }

        public void PlayEffectText(string content, Vector2 position)
        {
            var effectText = GetEffectText();
            effectText.ShowEffectText(content, position);
        }
        

        private TextEffect GetEffectText()
        {
            foreach (var effectText in _effectTexts)
            {
                if (!effectText.IsActive)
                    return effectText;
            }

            var newEffectText = Instantiate(prefab, transform);
            newEffectText.name = "Effect Text";
            _effectTexts.Add(newEffectText);
            return newEffectText;
        }
    }
}
