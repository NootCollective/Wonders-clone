using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationHand : CardLocation
{
    override public void Add(ActionCard card)
    {
        base.Add(card);
        card.transform.parent = this.transform;
        card.transform.localPosition = new Vector3((cards.Count-4) * 5, 0, 0);
        card.transform.localRotation = Quaternion.identity;
        card.Visible = true;
    }
}
