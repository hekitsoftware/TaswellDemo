using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

public class ItemManager : MonoBehaviour
{
    [Header("Item Slot")]
    [SerializeField] public Item tempItem;
    [SerializeField] public List<Item> _items = new List<Item>();

    [Header("Stats")]
    [SerializeField] public float DMG = 5f;
    [SerializeField] public float HP = 100f;
    [SerializeField] public float attackSPEED = 100f;

    public UnityEvent CompileState;

    [Header("Item Multipliers")]
    public float moveSpeedMulti = 1f; //!
    public float dmgMulti = 1f;
    public float hpMulti = 1f;
    public float attackSpeedMulti = 1f;
    public float jumpForce = 1f;

    #region Compile Items
    public void CompileItems()
    {
        float tempSpeed = 1f;
        float tempDMG = 1f;
        float tempHP = 1f;
        float tempAtkSpeed = 1f;
        float tempJumpForce = 1f;

        foreach (Item item in _items)
        {
            switch (item.GrowthCurve)
            {
                case GrowthCurve.Additive:
                    tempSpeed += item.SPEED;
                    tempDMG += item.DMG;
                    tempHP += item.HP;
                    tempAtkSpeed += item.atkSPEED;
                    tempJumpForce += item.JumpBalance;
                    break;

                case GrowthCurve.Multiplicative:
                    if (item.SPEED != 0) tempSpeed *= item.SPEED;
                    if (item.DMG != 0) tempDMG *= item.DMG;
                    if (item.HP != 0) tempHP *= item.HP;
                    if (item.atkSPEED != 0) tempAtkSpeed *= item.atkSPEED;
                    if (item.JumpBalance != 0) tempJumpForce *= item.JumpBalance;
                    break;

                case GrowthCurve.Percentage:
                    if (item.SPEED != 0) tempSpeed += (tempSpeed * item.SPEED);
                    if (item.DMG != 0) tempDMG += (tempDMG * item.DMG);
                    if (item.HP != 0) tempHP += (tempHP * item.HP);
                    if (item.atkSPEED != 0) tempAtkSpeed += (tempAtkSpeed * item.atkSPEED);
                    if (item.JumpBalance != 0) tempJumpForce += (tempJumpForce * item.JumpBalance);
                    break;
            }
        }

        moveSpeedMulti = tempSpeed;
        dmgMulti = tempDMG;
        hpMulti = tempHP;
        attackSpeedMulti = tempAtkSpeed;
        jumpForce = tempJumpForce;

        CompileState?.Invoke();
    }
    #endregion // Calculate Items
}
