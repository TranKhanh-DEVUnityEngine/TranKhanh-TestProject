
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupInfo : MonoBehaviour
{
    [SerializeField] private Image icons;
    [SerializeField] private ItemsPanel _itemsPanel;
    [SerializeField] private TextMeshProUGUI _titleItems;
    [SerializeField] private TextMeshProUGUI descText;
    private Callback _close;

    public void Enable(Callback close = null)
    {
        var items = _itemsPanel._shopItem;
        gameObject.SetActive(true);
        icons.sprite = GameControl.Instance.IconItems[items.id];
        _titleItems.text = items.title;
        descText.text = items.desc;
    }

    public void ButtonClose()
    {
        _close?.Invoke();
        gameObject.SetActive(false);
    }
}
