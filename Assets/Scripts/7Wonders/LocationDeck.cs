using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationDeck : CardLocation
{
    override public void Add(ActionCard card)
    {
        base.Add(card);
        card.transform.parent = this.transform;
        card.transform.localPosition = new Vector3(0, 0, -cards.Count*0.1f);
        card.transform.localRotation = Quaternion.identity;
        card.Visible = false;
    }
}
