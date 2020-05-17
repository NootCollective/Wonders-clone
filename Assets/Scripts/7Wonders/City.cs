using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public int Money;
    public int points;

    public List<ActionCard> options;
    public List<City> neighbors;

    public List<ActionCard> played;

    public bool ContainsCard(CardData other)
    {
        foreach (var card in played)
        {
            if (card.data == other)
            {
                return true;
            }
        }
        return false;
    }
    public bool ContainsCard(ActionCard other)
    {
        return ContainsCard(other.data);
    }
    void OnGUI()
    {
        Dictionary<ResourceType, int> count = new Dictionary<ResourceType, int>();
        foreach(var card in played)
        {
            foreach(var op in card.data.production)
            {
                foreach(var resource in op.content)
                {
                    if (!count.ContainsKey(resource))
                    {
                        count[resource] = 1;
                    }
                    else
                    {
                        ++count[resource];
                    }
                }
            }
        }
        int y = 0;
        foreach( var resource in count.Keys)
        {
            GUI.Label(new Rect(10, 10+y*20, 100, 20), resource.ToString()+"="+count[resource]);
            ++y;
        }
    }
    public bool CanPlay(ActionCard card)
    {
        if (ContainsCard(card))
        {
            return false;
        }
        if (card.data.cost.Length == 0)
        {
            return true;
        }
        else if (card.data.chainRequirement != null && ContainsCard(card.data.chainRequirement))
        {
            return true;
        }
        return false;

    }
    public bool Resolve(ActionCard card)
    {
        Debug.Log("No resolution done for now");
        return false;
    }
    public bool Play(ActionCard card)
    {
        if (CanPlay(card))
        {
            Resolve(card);
            played.Add(card);
            card.transform.parent = this.transform;
            card.transform.localPosition = new Vector3((played.Count - 1)*3, 1, 0);
            card.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }
        return false;
    }
}
