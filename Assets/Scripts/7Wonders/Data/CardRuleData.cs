using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardRule", menuName = "Data/Rule", order = 2)]
public class CardRuleData : ScriptableObject
{
    public enum RuleMoment
    {
        OnPlay, OnBuy, EndOfTurn, EndOfAge, EndOfGame
    }

    public RuleMoment ruleMoment;
    public bool[] countLMR = new bool[3];

    [Header("Special Production")]
    public bool countAndProduce;
    public CardType[] toCount;
    public int multiplier = 1;
    public ResourceType toGain = ResourceType.Money;

    [Header("Special Discount")]
    public bool discountOnPayment;
    public ResourceType[] discountedItems;

}
