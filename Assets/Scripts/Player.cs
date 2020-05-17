using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Money;
    public int points;

    public List<ActionCard> options;
    public List<Player> neighbors;

    public List<ActionCard> played;
}
