using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapModifier : MonoBehaviour
{
    public Grid grid;
    public Tilemap tilemap;
    public MapGenerator mapGen;

    public List<Vector3Int>[] citiesSlots;
    public List<Vector3Int>[] resourcesSlots;

    public static MapModifier Instance = null;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void OnValidate()
    {
        if (grid)
        {
            try
            {
                tilemap = grid.transform.Find("Tilemap").GetComponent<Tilemap>();
                mapGen = GetComponent<MapGenerator>();
            }
            catch
            { }
        }
    }


    public void PlaceBuilding(BuildingTile building, Vector3Int position)
    {
        if (building)
        {
            tilemap.SetTile(position, building.centerTile);

            if (building.ring1Tile)
            {
                var ring = HexagonalTile.GetHexagonalRing(position, 1);
                foreach (Vector3Int t in ring)
                {
                    if (Random.Range(0f, 1f) < building.ring1Probability)
                    {
                        tilemap.SetTile(t, building.ring1Tile);
                        if(Random.Range(0, 2) != 0)
                        {
                            Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 180f, 0f), Vector3.one);
                            tilemap.SetTransformMatrix(t, matrix);
                        }
                    }
                }
            }

            if (building.ring2Tile)
            {
                var ring = HexagonalTile.GetHexagonalRing(position, 2);
                foreach (Vector3Int t in ring)
                {
                    if (Random.Range(0f, 1f) < building.ring2Probability)
                    {
                        tilemap.SetTile(t, building.ring2Tile);
                    }
                }
            }
        }
    }



    public BuildingTile cityDebugTile;
    public BuildingTile resourceDebugTile;
    private void Start()
    {
        mapGen.GenerateMap();
        
        citiesSlots = new List<Vector3Int>[mapGen.citiesCenters.Length];
        resourcesSlots = new List<Vector3Int>[mapGen.citiesCenters.Length];
        for (int i = 0; i < mapGen.citiesCenters.Length; i++)
        {
            citiesSlots[i] = GetCitySlots(mapGen.citiesCenters[i]);
            resourcesSlots[i] = GetResourcesSlots(mapGen.citiesCenters[i]);
        }

        /*for (int i = 0; i < 6; i++)
        {
            foreach(Vector3Int v in citiesSlots[i])
                PlaceBuilding(cityDebugTile, v);
            foreach (Vector3Int v in resourcesSlots[i])
                PlaceBuilding(resourceDebugTile, v);
        }*/
    }


    public List<Vector3Int> GetCitySlots(Vector3Int cityCenter)
    {
        List<Vector3Int> slots = new List<Vector3Int>();
        
        if (cityCenter.y % 2 == 0)
        {
            slots.Add(new Vector3Int(cityCenter.x + 2, cityCenter.y - 1, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 3, cityCenter.y + 1, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x,     cityCenter.y - 3, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 1, cityCenter.y + 3, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 2, cityCenter.y - 2, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 2, cityCenter.y + 2, cityCenter.z));

            slots.Add(new Vector3Int(cityCenter.x + 5, cityCenter.y - 2, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 5, cityCenter.y + 2, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 1, cityCenter.y - 6, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 1, cityCenter.y + 6, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 4, cityCenter.y - 4, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 4, cityCenter.y + 4, cityCenter.z));

            slots.Add(new Vector3Int(cityCenter.x + 4, cityCenter.y + 1, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 5, cityCenter.y - 1, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 1, cityCenter.y + 5, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 2, cityCenter.y - 5, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 3, cityCenter.y + 4, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 3, cityCenter.y - 4, cityCenter.z));
        }
        else
        {
            slots.Add(new Vector3Int(cityCenter.x + 1, cityCenter.y - 3, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x ,    cityCenter.y + 3, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 3, cityCenter.y - 1, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 2, cityCenter.y + 1, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 2, cityCenter.y - 2, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 2, cityCenter.y + 2, cityCenter.z));

            slots.Add(new Vector3Int(cityCenter.x + 1, cityCenter.y - 6, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 1, cityCenter.y + 6, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 5, cityCenter.y - 2, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 5, cityCenter.y + 2, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 4, cityCenter.y - 4, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 4, cityCenter.y + 4, cityCenter.z));

            slots.Add(new Vector3Int(cityCenter.x + 2, cityCenter.y + 5, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 1, cityCenter.y - 5, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 3, cityCenter.y + 4, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 3, cityCenter.y - 4, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x + 5, cityCenter.y + 1, cityCenter.z));
            slots.Add(new Vector3Int(cityCenter.x - 4, cityCenter.y - 1, cityCenter.z));
        }

        return slots;
    }

    public List<Vector3Int> GetResourcesSlots(Vector3Int cityCenter)
    {
        List<Vector3Int> resources = new List<Vector3Int>();

        resources.Add(new Vector3Int(cityCenter.x-6, cityCenter.y+12, cityCenter.z));
        resources.Add(new Vector3Int(cityCenter.x+6, cityCenter.y+12, cityCenter.z));
        resources.Add(new Vector3Int(cityCenter.x-6, cityCenter.y-12, cityCenter.z));
        resources.Add(new Vector3Int(cityCenter.x+6, cityCenter.y-12, cityCenter.z));
        resources.Add(new Vector3Int(cityCenter.x-14, cityCenter.y, cityCenter.z));
        resources.Add(new Vector3Int(cityCenter.x+14, cityCenter.y, cityCenter.z));

        return resources;
    }
}
