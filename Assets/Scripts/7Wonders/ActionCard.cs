using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : MonoBehaviour
{
    public CardData data;

    public bool CanPlay(Player player)
    {
        return false;
    }
    public bool Play(Player player)
    {
        return false;
    }
}
