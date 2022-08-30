using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Do.Scripts.Tools.Text_Effect
{
    public class TextEffect : MonoBehaviour
    {
        [SerializeField] private TextEffectManager textEffectManager;
        [SerializeField] private TextMeshPro textMeshProUGUI;
        
        public bool IsActive { get; private set; }

        public void ShowEffectText(string content, Vector2 position)
        {
            IsActive = true;
            gameObject.SetActive(true);
            textMeshProUGUI.text = content;
            transform.position = position;
            position.y += 1f;
            transform.DOMoveY(position.y, textEffectManager.speed).SetEase(Ease.OutSine).OnComplete(delegate
            {
                IsActive = false;
                gameObject.SetActive(false);
            });
        }
    }
}
