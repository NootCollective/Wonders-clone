using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Wood, //0
    Stone, //1
    Ore, //2
    Clay, //3
    Glass, //4
    Paper, //5
    Textile, //6
    Money, //7
    Point, //8
    ScienceCompas, //9
    ScienceStone, //10
    ScienceGear, //11
    MilitaryShield//12
}

public class Resource : ScriptableObject
{
    public ResourceType type;


    public static Color[] resourceColor = new Color[13]
    {
        new Color(0.5f,0,0,1), //Wood
        new Color(0.5f,0.5f,0.5f,1),// Stone,
        new Color(0.5f,0.5f,0f,1),//Ore, 
        new Color(1f,0.5f,0,1),//Clay,
        new Color(0,0.25f,1f,1),//Glass,
        new Color(1,1,1f,1),//Paper, 
        new Color(1,0,1,1),//Textile,
        new Color(1,1,0f,1), //Money, 
        new Color(0,1,0,1),//Point, 
        new Color(0.25f,1,0.25f,1),//ScienceCompas, 
        new Color(0.25f,1,0.25f,1),//ScienceStone,
        new Color(0.25f,1,0.25f,1),//ScienceGear, 
        new Color(1,0,0,1), //MilitaryShield
    };

}
