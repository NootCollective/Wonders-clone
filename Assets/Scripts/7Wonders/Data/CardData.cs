using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Military, //contain "shield" symbols; these are added together to give a player's military strength, which is used in conflict resolution at the end of each age.
    Commercial, //have several effects: they can grant coins, resources, and/or victory points or decrease the cost of buying resources from neighbors.
    Scientific,// each card has one of three symbols. Combinations of the symbols are worth victory points.
    Civic, // [mistranslated as "civilian" in the game rules]): all grant a fixed number of victory points.
    MaterialRaw, //Brown cards (raw materials) provide one or two of the four raw material resources used in the game (wood, ore, clay brick, and stone).
    ManufacturedGood, //Grey cards (manufactured goods) provide one of the three manufactured goods used in the game (glass, papyrus, and textiles).
    Guild //Purple cards (guilds) generally grant victory points based on the structures a player and/or his neighbors have built.
}

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Card", order = 2)]
public class CardData : ScriptableObject
{
    public static Color[] CardColor = new Color[7]
   {
    Color.red,
    Color.yellow,
    Color.green,
    Color.blue,
    new Color(0.5f,0,0,1),
    Color.gray,
    new Color(148f/255,0,211f/255,1)
   };


    new public string name;
    public string description;
    public CardType type;

    [Header("Metadata")]
    public int ID;
    public Color cardColor;
    public int age;
    public int copies;

    [Header("Cost")]
    public ResourceType[] cost;
    public CardData chainRequirement;
    public CardData[] chainProvides;

    [Header("Value")]
    public OptionResource[] production;

    [Header("Additional rules")]
    public CardRuleData[] instantEffect;

    private void OnValidate()
    {
        Verify();
    }

    public void Verify()
    {
        cardColor = CardColor[(int)type];
    }
}
