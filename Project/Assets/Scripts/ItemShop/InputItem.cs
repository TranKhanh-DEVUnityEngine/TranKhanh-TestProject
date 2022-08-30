using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputItem", menuName = "InputItem")]
public class InputItem : ScriptableObject
{
    public List<ShopItem> inputBodyList;
}

[Serializable]
public class ShopItem
{
    public int id;
    public string icon;
    public string title;
    public string desc;
    public int price;
}
