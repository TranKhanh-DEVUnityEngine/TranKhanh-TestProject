using System;

[Serializable]
public class Price
{
    public BuyState State;
    public int Value;
}

public enum BuyState
{
    Coin,
    FishBone,
    InApp,
    Video,
    Gift,
    Daily
}