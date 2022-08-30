
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BodyItems : MonoBehaviour
{
    private ItemsPanel _manager;
    private int _id;
    [SerializeField] private Image Avartar;
    [SerializeField] private GameObject BtnBuy;
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _titleIcon;
    [SerializeField] private ShopItem _data;
    [SerializeField] private int _groupId;
    public void SetData(ShopItem data, int groupId, ItemsPanel manager)
    {
        _data = data;
        _manager = manager;
        _groupId = groupId;
      //  Locked.gameObject.SetActive(_data.State == State_Man.Locked);
       // Avartar.sprite = _data.Avartar;
      //  Avartar.SetNativeSize();
      Avartar.sprite = GameControl.Instance.IconItems[_data.id];
      _price.text = _data.price.ToString();
      _titleIcon.text = _data.title;
    }

    public void ShowBtn()
    {
        _manager.ClickBtn(_data);
    }
}
