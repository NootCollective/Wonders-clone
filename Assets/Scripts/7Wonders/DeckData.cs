using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Data/Deck", order = 2)]
public class DeckData : ScriptableObject
{
    public TextAsset file;
    public bool import;

    public List<CardData> cards;

    private char lineSeperater = '\n'; // It defines line seperate character
    private char fieldSeperator = ','; // It defines field seperate chracter

    private void OnValidate()
    {
        if (import)
        {
            import = false;
            ParseFile();
        }
    }

    void ParseFile()
    {
        cards = new List<CardData>();
        Dictionary<string, CardData> index = new Dictionary<string, CardData>();
        Dictionary<string, string> data = new Dictionary<string, string>();
        // if (file)
        {
            string text = file.text;
            string[] lines = text.Split(lineSeperater);
            for (int l = 2; l < lines.Length; ++l)
            {
                try
                {
                    string[] cardData = lines[l].Split(fieldSeperator);
                    CardData asset = ScriptableObject.CreateInstance<CardData>();
                    asset.name = cardData[4];
                    bool valid = int.TryParse(cardData[0], out asset.age);
                    if (valid)
                    {
                        UnityEditor.AssetDatabase.CreateAsset(asset, "Assets\\Data\\Cards\\" + asset.age+"."+asset.name + ".asset");

                        index[asset.name] = asset;
                        data[asset.name] = lines[l];
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Bad card description at " + l);
                }
            }

            int id = 0;
            foreach (string cardname in data.Keys)
            {
                string[] cardData = data[cardname].Split(fieldSeperator);
                CardData card = index[cardname];

                card.ID = id++;
                if (cardData[1] == "brown")
                {
                    card.type = CardType.MaterialRaw;
                }
                else if (cardData[1] == "gray")
                {
                    card.type = CardType.ManufacturedGood;
                }
                else if (cardData[1] == "blue")
                {
                    card.type = CardType.Civic;
                }
                else if (cardData[1] == "yellow")
                {
                    card.type = CardType.Commercial;
                }
                else if (cardData[1] == "red")
                {
                    card.type = CardType.Military;
                }
                else if (cardData[1] == "green")
                {
                    card.type = CardType.Scientific;
                }
                else if (cardData[1] == "violet")
                {
                    card.type = CardType.Guild;
                }
                else
                {
                    Debug.LogError("unknown card type " + cardData[1]);
                }

                if (cardData[2].Length > 0)
                {
                    try
                    {
                        card.chainRequirement = index[cardData[2]];
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("no indexed card with the name " + cardData[2]);
                    }

                }

                if (cardData[3].Length > 0)
                {
                    List<ResourceType> cost = new List<ResourceType>();
                    for(int c = 0; c < cardData[3].Length; ++c)
                    {
                        if(cardData[3][c] == 'W')
                        {
                            cost.Add(ResourceType.Wood);
                        }else if (cardData[3][c] == 'S')
                        {
                            cost.Add(ResourceType.Stone);
                        }
                        else if (cardData[3][c] == 'S')
                        {
                            cost.Add(ResourceType.Stone);
                        }
                        else if (cardData[3][c] == 'C')
                        {
                            cost.Add(ResourceType.Clay);
                        }
                        else if (cardData[3][c] == 'O')
                        {
                            cost.Add(ResourceType.Ore);
                        }
                        else if (cardData[3][c] == 'G')
                        {
                            cost.Add(ResourceType.Glass);
                        }
                        else if (cardData[3][c] == 'L')
                        {
                            cost.Add(ResourceType.Textile);
                        }
                        else if (cardData[3][c] == 'P')
                        {
                            cost.Add(ResourceType.Paper);
                        }
                        else
                        {
                            try
                            {
                                int money = int.Parse(cardData[3][c].ToString());
                                for(int m = 0;m < money; ++m)
                                {
                                    cost.Add(ResourceType.Money);
                                }
                            }
                            catch(System.Exception e)
                            {
                                Debug.LogError("Bad resource" + cardData[3][c]);
                            }
                        }
                    }
                    card.cost = cost.ToArray();
                }
                if (cardData[6].Length > 0)
                {

                    if (cardData[7].Length > 0)
                    {

                        card.chainProvides = new CardData[2];
                        try
                        {
                            card.chainProvides[0] = index[cardData[6]];
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("no indexed card with the name " + cardData[6]);
                        }
                        try
                        {
                            card.chainProvides[1] = index[cardData[7]];
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("no indexed card with the name " + cardData[7]);
                        }
                    }
                    else
                    {
                        card.chainProvides = new CardData[1];
                        try
                        {
                            card.chainProvides[0] = index[cardData[6]];
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("no indexed card with the name " + cardData[6]);
                        }
                    }
                }
                card.Verify();

                cards.Add(card);

            }
        }
    }
}
