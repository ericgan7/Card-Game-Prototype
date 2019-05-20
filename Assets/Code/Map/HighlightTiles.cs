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
    public Dictionary<Vector3Int, int> tilesFilled;
    Vector3Int[] neighbors;
    MapController map;
    List<Card.TargetType> validTags;

    public enum TileType
    {
        None, Move, Attack, Target, Path, Ally, Enemy
    }

    public Tile[] tiles;

    private void Start()
    {
        tilesFilled = new Dictionary<Vector3Int, int>();
        neighbors = new Vector3Int[4] { new Vector3Int(1,0,0), new Vector3Int(-1,0,0), new Vector3Int(0, 1,0), new Vector3Int(0,-1,0) };
        map = GetComponent<MapController>();
        validTags = new List<Card.TargetType>();
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
    //fills the straightline verticaly and horizontally
    public void FillCross(Vector3Int location, int range, TileType type, List<Card.TargetType> validTargets)
    {
        Vector4 valid = Vector4.one;
        for (int i = 0; i < range; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                Vector3Int t = location + neighbors[j] * i;
                if (valid[j] > 0 && map.WithinMapBounds(t))
                {
                    tilesFilled[t] = i;
                    highlights.SetTile(t, tiles[(int)type - 1]);
                }
            }
        }
    }
    //fills in tiles in a line
    public void FillRow(Vector3Int location, int range, TileType type, List<Card.TargetType> validTargets)
    {
        Vector4 valid = Vector4.one;
        List<Vector3Int> highlighted = map.highlights.GetTiles();
        for (int i = 0; i < range; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                Vector3Int t = location + neighbors[j] * i;
                if (highlighted.Contains(t) && valid[j] > 0)
                {
                    tilesFilled[t] = i;
                    highlights.SetTile(t, tiles[(int)type - 1]);
                }
                else
                {
                    valid[j] = 0;
                }
            }
        }
    }

    //Flood fill is used to fill in tiles from a certain distance from origin.
    public void FloodFill(Vector3Int origin, int area, TileType type, List<Card.TargetType> validtargets)
    {
        Clear();
        validTags.Clear();
        validTags = validtargets;
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
        if (remaining <= 0)
        {
            return;
        }
        foreach (Vector3Int n in neighbors)
        {
            Vector3Int newTile = location + n;
            if (!tilesFilled.ContainsKey(newTile))
            {
                if (map.WithinMapBounds(newTile) && (map.GetCharacter(newTile) == null || validTags.Contains(map.GetCharacter(newTile).team)))
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

    //Get All Highlighted Targets
    public List<Character> GetTargets()
    {
        List<Character> targets = new List<Character>();
        foreach (Vector3Int tile in tilesFilled.Keys)
        {
            Character c = map.GetCharacter(tile);
            if (c != null && c.team != Card.TargetType.PassableObstacle)
            {
                targets.Add(c);
            }
        }
        return targets;
    }

    //Get All Highlighted Tiles
    public List<Vector3Int> GetTiles()
    {
        List<Vector3Int> tiles = new List<Vector3Int>();
        foreach (Vector3Int t in tilesFilled.Keys)
        {
            tiles.Add(t);
        }
        return tiles;
    }
}
