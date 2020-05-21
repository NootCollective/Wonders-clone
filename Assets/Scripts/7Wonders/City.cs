using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : CardLocation
{
    public CardManager manager;
    public PlayerController player;
    public CityData data;
    public int Money;
    public int points;

    public LocationHand hand;
    public List<City> neighbors;

    public Dictionary<int, int> resources;

    public int cityID = -1;
    public List<int> freeCityBuildingSlots = new List<int>();
    public List<int> freeCityResourceSlots = new List<int>();

    public Transform construction;

    private void Start()
    {
        cityID = MapModifier.Instance.GetCityID();
        for(int i=0; i< MapModifier.Instance.citiesSlots[cityID].Count; i++)
        {
            freeCityBuildingSlots.Add(i);
        }
        for (int i = 0; i < MapModifier.Instance.resourcesSlots[cityID].Count; i++)
        {
            freeCityResourceSlots.Add(i);
        }
    }

    public int MilitaryStrength
    {
        get
        {
            ComputeOwnResources();
            int mID = resourceID(ResourceType.MilitaryShield);
            if (resources.ContainsKey(mID))
            {
                return resources[mID];
            }
            else
            {
                return 0;
            }
        }
    }
    public int resourceID(ResourceType resource)
    {
        return 2 << (int)resource;
    }
    public void ComputeOwnResources()
    {
        if (resources != null)
        {
            resources.Clear();
        }
        else
        {
            resources = new Dictionary<int, int>();
        }
        resources[resourceID(data.production)] = 1;
        resources[resourceID(ResourceType.Money)] = 1;
        foreach (var card in cards)
        {
            if (card.data.production.Length == 1)
            {
                var option = card.data.production[0];
                foreach (var resource in option.content)
                {
                    int resourceId = resourceID(resource);
                    if (!resources.ContainsKey(resourceId))
                    {
                        resources[resourceId] = 1;
                    }
                    else
                    {
                        ++resources[resourceId];
                    }
                }
            }
            else
            {
                int resourceId = 0;
                foreach (var option in card.data.production)
                {
                    foreach (var resource in option.content)
                    {
                        resourceId += 2 << (int)resource;
                    }
                    if (!resources.ContainsKey(resourceId))
                    {
                        resources[resourceId] = 1;
                    }
                    else
                    {
                        ++resources[resourceId];
                    }
                }
            }
        }
    }

    public bool ResolvePaymentOptions(ResourceType[] cost)
    {
       /* if (card.data.cost.Length == 0)
        {
            return true;
        }
        else
        {
            ComputeOwnResources();

            foreach (var c in card.cost.Keys)
            {
                List<ResourceType> missing = new List<ResourceType>();
                int costID = resourceID(c);
                if (!resources.ContainsKey(costID))
                {
                    missing.add
                }
                else if (resources[costID] < card.cost[c])
                {
                    return false;
                }
            }
            return true;
        }*/
        return false;
    }
    public bool CanPayWithOwnResources(ActionCard card)
    {
        if (card.data.cost.Length == 0)
        {
            return true;
        }
        else
        {
            ComputeOwnResources();

            foreach (var c in card.cost)
            {
                int costID = resourceID(c);
                if (!resources.ContainsKey(costID)
                    || resources[costID] < card.cost[c])
                {
                    return false;
                }
            }
            return true;
        }
    }
    public bool CanPlay(ActionCard card)
    {
        if (card == null || card.data == null)
        {
            Debug.LogError("invalid card:" + (card != null ? card.name : "no card"));
            return false;
        }
        else if (ContainsInstance(card.data))
        {
            return false;
        }
        else if (CanPayWithOwnResources(card))
        {
            return true;
        }
        else if (card.data.chainRequirement != null && ContainsInstance(card.data.chainRequirement))
        {
            return true;
        }
        return false;

    }
    public void Pay(ActionCard card)
    {
        if (card.cost[ResourceType.Money]>0)
        {
            this.Money -= card.cost[ResourceType.Money];
        }
    }
    public bool Resolve(ActionCard card)
    {
        Debug.Log("No resolution done for now");
        if (card && card.data && card.data.production.Length>0 && card.data.production[0]!=null && card.data.production[0].contentAsList[ResourceType.Money]>0)
        {
            Money += card.data.production[0].contentAsList[ResourceType.Money];

            return true;
        }
        return false;
    }
    public bool Play(ActionCard card)
    {
        if (CanPlay(card))
        {
            Pay(card);
            Resolve(card);
            hand.cards.Remove(card);
            Add(card);

            Build(card);

            return true;
        }
        return false;
    }

    public bool Discard(ActionCard card)
    {
        hand.Extract(card);
        Money += 3;
        manager.discards[card.data.age - 1].Add(card);
        return true;
    }
    public void BuildMarvel(ActionCard card)
    {
        // the card is turned into a building
        // ...

        //return false;
    }

    override public void Add(ActionCard card)
    {
        base.Add(card);
        card.transform.parent = construction;
        bool placed = false;
        if (card.data.chainRequirement != null)
        {
            var chain = GetInstance(card.data.chainRequirement);
            if (chain)
            {
                card.transform.localPosition = chain.transform.localPosition + new Vector3(1, -1.5f, 0);
                placed = true;
            }
        }
        if (!placed)
        {

            card.transform.localPosition = new Vector3((cards.Count - 3) * 5, 1, 0);
        }
        //card.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        card.Visible = true;
    }

    void Build(ActionCard card)
    {
        // the card is turned into a building
        // ...

        if (card.data.type == CardType.MaterialRaw)
        {
            foreach (var resource in card.data.production[0].contentAsList)
            {
                // this produces resource;
                // you can take the first one
            }

            int slotIndex = freeCityResourceSlots[Random.Range(0, freeCityResourceSlots.Count)];
            freeCityResourceSlots.Remove(slotIndex);
            MapModifier.Instance.PlaceBuilding(card.data.buildingTile, MapModifier.Instance.resourcesSlots[cityID][slotIndex]);
        }
        else if (card.data.type == CardType.ManufacturedGood)
        {
            foreach (var resource in card.data.production[0].contentAsList)
            {
                // this produces resource;
                // you can take the first one
            }

            int slotIndex = freeCityBuildingSlots[0];
            freeCityBuildingSlots.Remove(slotIndex);
            MapModifier.Instance.PlaceBuilding(card.data.buildingTile, MapModifier.Instance.citiesSlots[cityID][slotIndex]);
        }

        //...
    }
}
