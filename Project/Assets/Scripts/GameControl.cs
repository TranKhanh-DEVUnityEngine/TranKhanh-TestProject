
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Do.Scripts.Tools.Other;
using UnityEngine;


public class GameControl : Singleton<GameControl>
{
    public List<Sprite> IconItems;
    [SerializeField] private InputItem inputItem;
    public List<ShopItem> ItemsInfos => inputItem.inputBodyList;

   // public InputItem ShopItem = new InputItem();
    private void OnEnable()
    {
    //  Convert();
    }
    [ContextMenu("Convert Data")]
    private void Convert()
    {
        var path = File.ReadAllText(Application.dataPath + "/Resources/ShopItems.json");
        var data = GameData.JsonHelper.FromJson<ShopItem>(path);
        inputItem.inputBodyList = new List<ShopItem>();
        inputItem.inputBodyList = data.ToList();
    }
}