using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood, Stone, Ore, Clay, Glass, Paper, Textile, Money, Point, ScienceCompas, ScienceStone, ScienceGear, MilitaryShield
}

public class Resource : ScriptableObject
{
    public ResourceType type;


    public static Color[] resourceColor = new Color[13]
    {
        new Color(0.5f,0,0,1), //Wood
        new Color(0.5f,0.5f,0.5f,1),// Stone,
        new Color(0.5f,0.5f,0.5f,1),//Ore, 
        new Color(1f,0.5f,0.5f,1),//Clay,
        new Color(0,0.25f,1f,1),//Glass,
        new Color(1,1,1f,1),//Paper, 
        new Color(1,1,0,1),//Textile,
        new Color(1,0,1f,1), //Money, 
        new Color(0,1,0,1),//Point, 
        new Color(0.25f,1,0.25f,1),//ScienceCompas, 
        new Color(0.25f,1,0.25f,1),//ScienceStone,
        new Color(0.25f,1,0.25f,1),//ScienceGear, 
        new Color(1,0,0,1), //MilitaryShield
    };

}
