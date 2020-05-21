using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckData", menuName = "Data/Deck", order = 2)]
public class DeckData : ScriptableObject
{
    public TextAsset file;
    public bool import;

    public List<CardData> cards;
    public List<CardData> age1;
    public List<CardData> age2;
    public List<CardData> age3;
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

    CardData.OptionResource ParseProduction(string productionDescriptor)
    {
        CardData.OptionResource production = new CardData.OptionResource();
        production.content = new List<ResourceType>();
        for (int c = 0; c < productionDescriptor.Length; ++c)
        {
            if (productionDescriptor[c] == ' ')
            {
                continue;
            }
            else if (productionDescriptor[c] == 'W')
            {
                production.content.Add(ResourceType.Wood);
            }
            else if (productionDescriptor[c] == 'S')
            {
                production.content.Add(ResourceType.Stone);
            }
            else if (productionDescriptor[c] == 'S')
            {
                production.content.Add(ResourceType.Stone);
            }
            else if (productionDescriptor[c] == 'C')
            {
                production.content.Add(ResourceType.Clay);
            }
            else if (productionDescriptor[c] == 'O')
            {
                production.content.Add(ResourceType.Ore);
            }
            else if (productionDescriptor[c] == 'G')
            {
                production.content.Add(ResourceType.Glass);
            }
            else if (productionDescriptor[c] == 'L')
            {
                production.content.Add(ResourceType.Textile);
            }
            else if (productionDescriptor[c] == '&')
            {
                production.content.Add(ResourceType.ScienceCompas);
            }
            else if (productionDescriptor[c] == '@')
            {
                production.content.Add(ResourceType.ScienceGear);
            }
            else if (productionDescriptor[c] == '#')
            {
                production.content.Add(ResourceType.ScienceStone);
            }
            else if (productionDescriptor[c] == 'P')
            {
                production.content.Add(ResourceType.Paper);
            }
            else if (productionDescriptor[c] == 'X')
            {
                production.content.Add(ResourceType.MilitaryShield);
            }
            else if (productionDescriptor[c] == '{')
            {
                try
                {
                    int points = int.Parse(productionDescriptor[c + 1].ToString());
                    for (int m = 0; m < points; ++m)
                    {
                        production.content.Add(ResourceType.Point);
                    }
                    c += 2;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("  Bad resource at '" + productionDescriptor + "'[" + c + "]" + productionDescriptor[c] + productionDescriptor[c + 1] + productionDescriptor[c + 2]);
                    production = null;
                    break;
                }
            }
            else
            {
                try
                {
                    int money = int.Parse(productionDescriptor[c].ToString());
                    for (int m = 0; m < money; ++m)
                    {
                        production.content.Add(ResourceType.Money);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("  Bad resource" + productionDescriptor[c]);
                    production.content.Clear();
                    production = null;
                    break;
                }
            }
        }
        return production;
    }
    void ParseFile()
    {
        cards = new List<CardData>();
        age1 = new List<CardData>();
        age2 = new List<CardData>();
        age3 = new List<CardData>();
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
                        UnityEditor.AssetDatabase.CreateAsset(asset, "Assets\\Data\\Cards\\" + asset.age + "." + asset.name + ".asset");

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

                //TODO: take into account amount of players
                bool hasCopies = int.TryParse(cardData[12], out card.copies);
                if (!hasCopies)
                {
                    Debug.LogWarning("No copy information");
                    card.copies = 1;
                }

                card.ID = id++;
                #region type
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
                #endregion
                #region cost
                string costDescriptor = cardData[3];
                if (costDescriptor.Length > 0)
                {
                    List<ResourceType> cost = new List<ResourceType>();
                    for (int c = 0; c < costDescriptor.Length; ++c)
                    {
                        if (costDescriptor[c] == 'W')
                        {
                            cost.Add(ResourceType.Wood);
                        }
                        else if (costDescriptor[c] == 'S')
                        {
                            cost.Add(ResourceType.Stone);
                        }
                        else if (costDescriptor[c] == 'S')
                        {
                            cost.Add(ResourceType.Stone);
                        }
                        else if (costDescriptor[c] == 'C')
                        {
                            cost.Add(ResourceType.Clay);
                        }
                        else if (costDescriptor[c] == 'O')
                        {
                            cost.Add(ResourceType.Ore);
                        }
                        else if (costDescriptor[c] == 'G')
                        {
                            cost.Add(ResourceType.Glass);
                        }
                        else if (costDescriptor[c] == 'L')
                        {
                            cost.Add(ResourceType.Textile);
                        }
                        else if (costDescriptor[c] == 'P')
                        {
                            cost.Add(ResourceType.Paper);
                        }
                        else
                        {
                            try
                            {
                                int money = int.Parse(costDescriptor[c].ToString());
                                for (int m = 0; m < money; ++m)
                                {
                                    cost.Add(ResourceType.Money);
                                }
                            }
                            catch (System.Exception e)
                            {
                                Debug.LogError("Bad resource" + costDescriptor[c]);
                            }
                        }
                    }
                    card.cost = cost.ToArray();
                }
                #endregion
                #region chain
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
                #endregion
                #region production
                if (cardData[5].Length > 0)
                {
                    string productionDescriptor = cardData[5];
                    if (productionDescriptor.Contains("/"))
                    {
                        string[] options = productionDescriptor.Split('/');
                        try
                        {
                            card.production = new CardData.OptionResource[options.Length];
                            for(int p = 0; p < options.Length; ++p)
                            {
                                var production = ParseProduction(options[p]);
                                card.production[p] = production;
                            }
                            
                        }
                        catch(System.Exception e)
                        {
                            Debug.LogError("Bad options " + productionDescriptor);
                            card.production = new CardData.OptionResource[0];
                        }
                    }
                    else
                    {
                        card.production = new CardData.OptionResource[1];
                        card.production[0] = ParseProduction(productionDescriptor);
                    }

                }
                #endregion

                card.Verify();

                cards.Add(card);

                switch (card.age)
                {
                    case 1:
                        age1.Add(card);
                        break;
                    case 2:
                        age2.Add(card);
                        break;
                    case 3:
                        age3.Add(card);
                        break;
                }
            }
        }
    }
}
