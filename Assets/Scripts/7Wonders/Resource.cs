using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood, Stone, Ore, Clay, Glass, Paper, Textile, Money
}

public class Resource : ScriptableObject
{
    public ResourceType type;
}
