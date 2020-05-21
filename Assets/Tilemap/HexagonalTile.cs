using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HexagonalTile : Tile
{

    public static Vector3Int GetNeighbor(Vector3Int cell, int n)
    {
        if (cell.y % 2 == 0)
        {
            switch (n)
            {
                case 0: return new Vector3Int(cell.x - 1, cell.y + 1, cell.z);
                case 1: return new Vector3Int(cell.x - 1, cell.y, cell.z);
                case 2: return new Vector3Int(cell.x - 1, cell.y - 1, cell.z);
                case 3: return new Vector3Int(cell.x, cell.y - 1, cell.z);
                case 4: return new Vector3Int(cell.x + 1, cell.y, cell.z);
                case 5: return new Vector3Int(cell.x, cell.y + 1, cell.z);
            }
        }
        else
        {
            switch (n)
            {
                case 0: return new Vector3Int(cell.x, cell.y + 1, cell.z);
                case 1: return new Vector3Int(cell.x, cell.y, cell.z);
                case 2: return new Vector3Int(cell.x, cell.y - 1, cell.z);
                case 3: return new Vector3Int(cell.x + 1, cell.y - 1, cell.z);
                case 4: return new Vector3Int(cell.x + 1, cell.y, cell.z);
                case 5: return new Vector3Int(cell.x + 1, cell.y + 1, cell.z);
            }

            return cell;
        }
        return cell;
    }

    public static Vector3Int[] GetNeibors(Vector3Int cell)
    {
        Vector3Int[] neigbors = new Vector3Int[6];
        if (cell.y % 2 == 0)
        {
            neigbors[0] = new Vector3Int(cell.x - 1, cell.y + 1, cell.z);
            neigbors[1] = new Vector3Int(cell.x - 1, cell.y, cell.z);
            neigbors[2] = new Vector3Int(cell.x - 1, cell.y - 1, cell.z);
            neigbors[3] = new Vector3Int(cell.x, cell.y - 1, cell.z);
            neigbors[4] = new Vector3Int(cell.x + 1, cell.y, cell.z);
            neigbors[5] = new Vector3Int(cell.x, cell.y + 1, cell.z);
        }
        else
        {
            neigbors[0] = new Vector3Int(cell.x, cell.y + 1, cell.z);
            neigbors[1] = new Vector3Int(cell.x - 1, cell.y, cell.z);
            neigbors[2] = new Vector3Int(cell.x, cell.y - 1, cell.z);
            neigbors[3] = new Vector3Int(cell.x + 1, cell.y - 1, cell.z);
            neigbors[4] = new Vector3Int(cell.x + 1, cell.y, cell.z);
            neigbors[5] = new Vector3Int(cell.x + 1, cell.y + 1, cell.z);
        }
        return neigbors;
    }

    public static Vector3Int GetRingCorner(Vector3Int cell, int n, int radius)
    {
        if (radius <= 0)
        {
            return cell;
        }

        for (int i = 0; i < radius; i++)
        {
            cell = GetNeighbor(cell, n);
        }
        
        return cell;
    }
    
    public static Vector3Int[] GetHexagonalRing(Vector3Int center, int radius)
    {
        if (radius == 2)
        {
            if (center.y % 2 == 0)
            {
                Vector3Int[] ring = {
                    new Vector3Int(center.x-1, center.y-2, center.z),
                    new Vector3Int(center.x,   center.y-2, center.z),
                    new Vector3Int(center.x+1, center.y-2, center.z),
                    new Vector3Int(center.x+1, center.y-1, center.z),
                    new Vector3Int(center.x+2, center.y,   center.z),
                    new Vector3Int(center.x+1, center.y+1, center.z),
                    new Vector3Int(center.x+1, center.y+2, center.z),
                    new Vector3Int(center.x,   center.y+2, center.z),
                    new Vector3Int(center.x-1, center.y+2, center.z),
                    new Vector3Int(center.x-2, center.y+1, center.z),
                    new Vector3Int(center.x-2, center.y,   center.z),
                    new Vector3Int(center.x-2, center.y-1, center.z)
                };
                return ring;
            }
            else
            {
                Vector3Int[] ring = {
                    new Vector3Int(center.x-1, center.y-2, center.z),
                    new Vector3Int(center.x,   center.y-2, center.z),
                    new Vector3Int(center.x+1, center.y-2, center.z),
                    new Vector3Int(center.x+2, center.y-1, center.z),
                    new Vector3Int(center.x+2, center.y,   center.z),
                    new Vector3Int(center.x+2, center.y+1, center.z),
                    new Vector3Int(center.x+1, center.y+2, center.z),
                    new Vector3Int(center.x,   center.y+2, center.z),
                    new Vector3Int(center.x-1, center.y+2, center.z),
                    new Vector3Int(center.x-1, center.y+1, center.z),
                    new Vector3Int(center.x-2, center.y,   center.z),
                    new Vector3Int(center.x-1, center.y-1, center.z)
                };
                return ring;
            }
        }
        else if(radius == 1)
        {
            return GetNeibors(center);
        }
        else
        {
            Vector3Int[] ring = { center };
            return ring;
        }
    }
}
