using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : MonoBehaviour, IEquatable<ActionCard>
{
    public CardData data;
    public CardLocation location;
    new public CardRenderer renderer;

    static int MAXID = 0;
    public int ID = ++MAXID;

    [SerializeField] bool visible = true;
    public bool Visible
    {
        set
        {
            visible = value;
            renderer.Redraw();
        }
        get
        {
            return visible;
        }
    }
    public Dictionary<ResourceType, int> cost;

    public void Initialize()
    {
        ComputeCost();
    }
    public void ComputeCost()
{
        cost = new Dictionary<ResourceType, int>();
        foreach (var c in data.cost)
        {
            if (!cost.ContainsKey(c))
            {
                cost[c] = 1;
            }
            else
            {
                ++cost[c];
            }
        }
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as ActionCard);
    }

    public bool Equals(ActionCard other)
    {
        return other != null &&
               base.Equals(other) &&
               ID == other.ID;
    }

    public override int GetHashCode()
    {
        var hashCode = 2082127350;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + ID.GetHashCode();
        return hashCode;
    }
}
