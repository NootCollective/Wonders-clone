using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("Map settings")]
    public BoundsInt mapSize = new BoundsInt(new Vector3Int(-5, -5, 0), new Vector3Int(10, 10, 1));
    public Grid grid;
    public Tilemap tilemap;
    public TileBase backGroundTile;
    public bool generateOnValidate = true;
    public bool generateOnStart = true;
    public bool backgroundFill = false;

    [Header("Resources tile lists")]
    public TileBase forestTile;
    public TileBase stoneTile;
    public TileBase mineralTile;
    public TileBase clayTile;

    [Header("Traveling ways tiles")]
    public TileBase waterTile;
    public TileBase roadTile;

    [Header("Cities settings")]
    public int citiesDistances = 50;
    public int cityRadius = 10;
    public float waysDisplacementNoise = 10f;
    public int waysDecimation = 4;
    public AnimationCurve forestProbability;
    public float forestThreshold = 0.2f;

    private void OnValidate()
    {
        if (grid)
        {
            try
            {
                tilemap = grid.transform.Find("Tilemap").GetComponent<Tilemap>();
            }
            catch
            { }
        }

        if(grid && tilemap && generateOnValidate)
        {
            GenerateMap();
        }
    }
    private void Start()
    {
        if(generateOnStart)
            GenerateMap();
    }

    private void GenerateMap()
    {
        // background
        if (backgroundFill && backGroundTile)
        {
            List<TileBase> tiles = new List<TileBase>();
            foreach (Vector3Int p in mapSize.allPositionsWithin)
            {
                //tilemap.SetTile(p, backGroundTile);
                tiles.Add(backGroundTile);
            }
            tilemap.SetTilesBlock(mapSize, tiles.ToArray());
        }

        // forest
        Vector3Int[] citiesCenters = new Vector3Int[6];
        tilemap.SetTile(new Vector3Int(0, 0, 0), waterTile);
        for (int i = 0; i < citiesCenters.Length; i++) 
        {
            float angle = 60 * i * Mathf.Deg2Rad;
            Vector3 p1 = new Vector3(citiesDistances * Mathf.Cos(angle), citiesDistances * Mathf.Sin(angle), 0);
            citiesCenters[i] = new Vector3Int((int)p1.x, (int)p1.y, (int)p1.z);
            
            BoundsInt cityBound = new BoundsInt(citiesCenters[i] - new Vector3Int(cityRadius, cityRadius, 0), new Vector3Int(2 * cityRadius, 2 * cityRadius, 1));

            foreach (Vector3Int cell in cityBound.allPositionsWithin)
            {
                Vector3 p2 = new Vector3(cell.x, cell.y, cell.z);
                float r = Vector3.Distance(p1, p2);

                if (r < cityRadius && RandomForest(r / cityRadius) > forestThreshold)
                {
                    tilemap.SetTile(cell, forestTile);
                }
                else if (r < cityRadius)
                {
                    tilemap.SetTile(cell, backGroundTile);
                }
            }
            tilemap.SetTile(citiesCenters[i], waterTile);
        }
        
        // rivers
        for (int i = 0; i < citiesCenters.Length; i++)
        {
            Vector3Int start = citiesCenters[i];
            List<Vector3Int> path = GetNoisyPath(start, new Vector3Int(0, 0, 0), waysDecimation, waysDisplacementNoise);
            foreach (Vector3Int p2 in path)
            {

                tilemap.SetTile(p2, waterTile);
            }

            float angle = 60 * i * Mathf.Deg2Rad;
            Vector3 p1 = new Vector3(2 * citiesDistances * Mathf.Cos(angle), 2 * citiesDistances * Mathf.Sin(angle), 0);
            List<Vector3Int> path2 = GetNoisyPath(start, new Vector3Int((int)p1.x, (int)p1.y, (int)p1.z), waysDecimation, waysDisplacementNoise);
            foreach (Vector3Int p2 in path2)
            {
                tilemap.SetTile(p2, waterTile);
            }
        }
        
        // roads
        for(int i=0; i< citiesCenters.Length; i++)
        {
            Vector3Int start = citiesCenters[i];
            Vector3Int end = citiesCenters[(i + 1)% citiesCenters.Length];
            List<Vector3Int> path = GetNoisyPath(start, end, waysDecimation, waysDisplacementNoise);
            foreach(Vector3Int p2 in path)
            {
                tilemap.SetTile(p2, roadTile);
            }
        }
    }

    private List<Vector3Int> GetNoisyPath(Vector3Int start, Vector3Int end, int segmentCount, float displacement = 1f)
    {
        segmentCount = Mathf.Max(segmentCount, 1);
        Vector3Int[] segments = new Vector3Int[segmentCount + 1];
        segments[0] = start;
        segments[segmentCount] = end;
        Vector3 normalDirection = new Vector3(end.y - start.y, start.x - end.x, 0).normalized;
        for (int i = 1; i < segments.Length-1; i++)
        {
            Vector3 dp = Random.Range(-displacement, displacement) * normalDirection;
            segments[i] = LerpCell(start, end, (float)i / segmentCount) + new Vector3Int((int)dp.x, (int)dp.y, (int)dp.z);
        }
        
        List<Vector3Int> path = new List<Vector3Int>();
        for (int i = 0; i < segments.Length-1; i++) 
        {
            path.AddRange(GetSegmentPath(segments[i], segments[(i + 1) % segments.Length]));
        }

        return path;
    }
    private List<Vector3Int> GetSegmentPath(Vector3Int start, Vector3Int end)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        int stopIteration = 200;
        Vector3Int cell = start;
        while (cell != end)
        {
            path.Add(cell);
            Vector3 direction = new Vector3(end.x - cell.x, end.y - cell.y, end.z - cell.z);
            cell = NeighborInDirection(cell, direction);

            // security
            stopIteration--;
            if (stopIteration <= 0)
            {
                break;
            }
        }
        path.Add(end);
        return path;
    }
    private Vector3Int GetNeighbor(Vector3Int cell, int n)
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
                case 1: return new Vector3Int(cell.x - 1, cell.y, cell.z);
                case 2: return new Vector3Int(cell.x, cell.y - 1, cell.z);
                case 3: return new Vector3Int(cell.x + 1, cell.y - 1, cell.z);
                case 4: return new Vector3Int(cell.x + 1, cell.y, cell.z);
                case 5: return new Vector3Int(cell.x + 1, cell.y + 1, cell.z);
            }
        }
        return cell;
    }
    private Vector3Int NeighborInDirection(Vector3Int cell, Vector3 direction)
    {
        float maxd = float.MinValue;
        Vector3Int winner = new Vector3Int();
        for (int i=0; i<6; i++)
        {
            Vector3Int n = GetNeighbor(cell, i);
            float d = Vector3.Dot(direction, new Vector3(n.x - cell.x, n.y - cell.y, n.z - cell.z));

            if (d > maxd)
            {
                winner = n;
                maxd = d;
            }
        }
        return winner;
    }
    private Vector3Int LerpCell(Vector3Int start, Vector3Int end, float t)
    {
        Vector3 v = Vector3.Lerp(start, end, t);
        return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
    }
    public float RandomForest(float t)
    {
        return Random.Range(0f, forestProbability.Evaluate(t));
    }
}
