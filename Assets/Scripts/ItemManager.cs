using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Item Slot")]
    [SerializeField] public Item tempItem;
    [SerializeField] public static List<Item> _items = new List<Item>();

    [Header("Stats")]
    [SerializeField] public float DMG = 5f;
    [SerializeField] public float HP = 100f;
    [SerializeField] public float attackSPEED = 100f;

    [Header("Item Multipliers")]
    public float moveSpeedMulti = 1f;
    public float dmgMulti = 1f;
    public float hpMulti = 1f;
    public float attackSpeedMulti = 1f;

    public void CompileItems()
    {
        float tempSpeed = 1f;
        float tempDMG = 1f;
        float tempHP = 1f;
        float tempAtkSpeed = 1f;

        //cycle through items
        foreach (Item item in _items)
        {
            //sort through growth types
            switch (item.GrowthCurve)
            {
                case GrowthCurve.Additive:
                    tempSpeed += item.SPEED;
                    tempDMG += item.DMG;
                    tempHP += item.HP;
                    tempAtkSpeed += item.atkSPEED;
                    break;

                //if variable != 0, temp var = (temp var * item.var); else: temp var = (temp var * 1)
                case GrowthCurve.Multiplicative:
                    tempSpeed = item.SPEED != 0 ? (tempSpeed * item.SPEED) : 1f;
                    tempDMG = item.DMG != 0 ? (tempDMG * item.DMG) : 1f;
                    tempHP = item.HP != 0 ? (tempHP * item.HP) : 1f;
                    tempAtkSpeed = item.atkSPEED != 0 ? (tempAtkSpeed * item.atkSPEED) : 1f;
                    break;

                case GrowthCurve.Percentage:
                    tempSpeed = item.SPEED != 0 ? (tempSpeed + (tempSpeed + ((item.SPEED / tempSpeed) * 100))) : 1f;
                    tempDMG = item.DMG != 0 ? (tempDMG + (tempDMG + ((item.DMG / tempDMG) * 100))) : 1f;
                    tempHP = item.HP != 0 ? (tempHP + (tempHP + ((item.HP / tempHP) * 100))) : 1f;
                    tempAtkSpeed = item.atkSPEED != 0 ? (tempAtkSpeed + (tempAtkSpeed + ((item.atkSPEED / tempAtkSpeed) * 100))) : 1f;
                    break;
            }
        }
    }
}
