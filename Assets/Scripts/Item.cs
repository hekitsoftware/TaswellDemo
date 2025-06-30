using UnityEngine;

public enum RarityType
{
    White,
    Green,
    Purple,
    Red,
}

public enum GrowthCurve
{
    Additive,
    Multiplicative,
    Percentage,
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Item Info")]
    [SerializeField] public string itemName;
    [SerializeField] public string itemDesc;
    [SerializeField] public Sprite itemSprite;
    public RarityType Rarity;
    public GrowthCurve GrowthCurve;

    [Header("Player Stats")]
    [SerializeField] public float HP;
    [SerializeField] public float SPEED;
    [SerializeField] public float DMG;
    [SerializeField] public float atkSPEED;

    [Header("World Stats")]
    [SerializeField] public float PowerTime;
    [SerializeField] public float GravityScale;
}
