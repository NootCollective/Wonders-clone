using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CityData", menuName = "Data/City", order = 2)]
public class CityData : ScriptableObject
{
    new public string name;
    public Color color;
    public bool side = false;
    public ResourceType production;
    public CardData[] marvel;
}
