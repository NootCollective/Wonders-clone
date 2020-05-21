using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardRule", menuName = "Data/Rule", order = 2)]
public class CardRuleData : ScriptableObject
{
    public enum RuleMoment
    {
        OnPlay, EndOfTurn, EndOfAge, EndOfGame
    }
    public RuleMoment ruleMoment;

    [Header("Special Production")]
    public bool produceSpecial;
    public bool[] countLMR = new bool[3];
    public ResourceType[] toCount;
    public int multiplier = 1;
    public ResourceType toGain = ResourceType.Money;
}
