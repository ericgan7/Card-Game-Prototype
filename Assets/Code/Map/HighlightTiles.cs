using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Used to highlihgt tilemap grid for attack and movement indicators.
/// </summary>

public class HighlightTiles : MonoBehaviour
{
    public Tilemap highlights;
    Dictionary<Vector3Int, int> tilesFilled;
    Vector3Int[] neighbors;
    MapController map;

    public enum TileType
    {
        None, Move, Attack, Target, Buff
    }

    public Tile[] tiles;

    private void Start()
    {
        tilesFilled = new Dictionary<Vector3Int, int>();
        neighbors = new Vector3Int[4] { new Vector3Int(1,0,0), new Vector3Int(-1,0,0), new Vector3Int(0, 1,0), new Vector3Int(0,-1,0) };
        map = GetComponent<MapController>();
    }

    //Change a tile to a specific tile type. Can be used to set to none.
    public void ChangeTile(Vector3Int location, TileType t)
    {
        if (t == TileType.None)
        {
            tilesFilled.Remove(location);
            highlights.SetTile(location, null);
        }
        else
        {
            tilesFilled[location] = 1;
            highlights.SetTile(location, tiles[(int)t - 1]);
        }
    }

    //Clears out entire tilemap
    public void Clear()
    {
        foreach(Vector3Int location in tilesFilled.Keys)
        {
            highlights.SetTile(location, null);
        }
        tilesFilled.Clear();
    }
    //Checks if a tile is highlighted. Can be used to check if it is a valid movement space or valid target location;
    public bool Contains(Vector3Int location)
    {
        return tilesFilled.ContainsKey(location);
    }
    //Flood fill is used to fill in tiles from a certain distance from origin.
    public void FloodFill(Vector3Int origin, int area, TileType type)
    {
        Clear();

        Flood(origin, area);
        foreach(Vector3Int location in tilesFilled.Keys)
        {
            highlights.SetTile(location, tiles[(int)type - 1]);
        }
    }
    //recursive flooding.
    public void Flood(Vector3Int location, int remaining)
    {
        tilesFilled[location] = remaining;
        --remaining;
        if (remaining == 0)
        {
            return;
        }
        foreach (Vector3Int n in neighbors)
        {
            Vector3Int newTile = location + n;
            if (!tilesFilled.ContainsKey(newTile))
            {
                if (map.WithinMapBounds(newTile))
                {
                    Flood(newTile, remaining);
                }
            }
            else if (tilesFilled[newTile] < remaining)
            {
                Flood(newTile, remaining);
            }
        }
    }

}
