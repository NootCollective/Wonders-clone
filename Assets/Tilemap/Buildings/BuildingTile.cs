using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "BuildingTile", menuName = "Data/Building", order = 3)]
public class BuildingTile : HexagonalTile
{
    public TileBase centerTile;
    public TileBase[] ring1Tile;
    public TileBase[] ring2Tile;

    public float ring1Probability;
    public float ring2Probability;

    public bool flipAllowed = true;
}
