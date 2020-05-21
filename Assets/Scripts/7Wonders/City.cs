using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : CardLocation
{
    public PlayerController player;
    public CityData data;
    public int Money;
    public int points;

    public LocationHand hand;
    public List<City> neighbors;

    public Dictionary<int, int> resources;

    public Transform construction;
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

    public bool CanPayWithOwnResources(ActionCard card)
    {
        if (card.data.cost.Length == 0)
        {
            return true;
        }
        else
        {
            ComputeOwnResources();

            foreach (var c in card.cost.Keys)
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
        if (card.cost.ContainsKey(ResourceType.Money))
        {
            this.Money -= card.cost[ResourceType.Money];
        }
    }
    public bool Resolve(ActionCard card)
    {
        Debug.Log("No resolution done for now");
        /*if (card)
        {
            Money += card.production.money;
        }*/
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

        }
        return false;
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
}
