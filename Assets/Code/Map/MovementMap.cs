using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMap : MonoBehaviour
{
    public int[,] movemap;
    public int[,] threatmap;
    MapController mc;
    Vector2Int[] neighbors;
    int mapx;
    int mapy;
    HashSet<Vector2Int> impasable;

    private void Start()
    {
        mc = GetComponent<MapController>();
        mapx = mc.mapx;
        mapy = mc.mapy;
        movemap = new int[mapx, mapy];
        threatmap = new int[mapx, mapy];
        neighbors = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        impasable = new HashSet<Vector2Int>();
    }

    public Vector2Int NextMove(Vector3Int location)
    {
        int current = movemap[location.x, location.y];
        Vector2Int next = new Vector2Int(-1, -1);
        foreach (Vector2Int n in neighbors)
        {
            Vector2Int newLoc = (Vector2Int)location + n;
            if (mc.WithinMapBounds((Vector3Int)newLoc) && movemap[newLoc.x, newLoc.y] < current)
            {
                next = newLoc;
                current = movemap[newLoc.x, newLoc.y];
            }
        }
        return next;
    }

    //Dijkstra maps to help units stay within a certain range of characters.
    //parameter is desired range - 1;
    public void UpdateMovementMap(Character self, int range)
    {
        //reset maps;
        for (int x = 0; x < mapx; ++x)
        {
            for (int y = 0; y < mapy; ++y)
            {
                movemap[x, y] = 20;
                threatmap[x, y] = 100;
            }
        }
        impasable.Clear();
        //assign goals
        List<Vector3Int> goals = mc.GetCharacterLocations();
        foreach (Vector3Int v in goals)
        {
            if (v.z == -1)
            {
                impasable.Add(new Vector2Int(v.x, v.y));
            }
            else
            {
                movemap[v.x, v.y] = 0;
                threatmap[v.x, v.y] = v.z + 1;
            }
        }
        //update maps until it stops changing
        bool changed = true;
        int iter = 0;
        while (changed)
        {
            changed = DijkstraMap(movemap);
            ++iter;
            if (iter > 20)
            {
                Debug.Log(debug());
                break;
            }
        }
        iter = 0;
        changed = true;
        while (changed)
        {
            changed = DijkstraMap(threatmap);
            ++iter;
            if (iter > 20)
            {
                Debug.Log("DijkstraMap Error");
                break;
            }
        }
        FlipRange(range);
        Combine();
    }

    public bool DijkstraMap(int[,] map)
    {
        bool changed = false;
        for (int x = 0; x < mapx; ++x)
        {
            for (int y = 0; y < mapy; ++y)
            {
                Vector2Int loc = new Vector2Int(x, y);
                if (!impasable.Contains(loc))
                {
                    foreach (Vector2Int n in neighbors)
                    {
                        Vector2Int newLoc = loc + n;
                        if (mc.WithinMapBounds((Vector3Int)newLoc) && map[newLoc.x, newLoc.y] + 1 < map[loc.x, loc.y])
                        {
                            changed = true;
                            map[loc.x, loc.y] = map[newLoc.x, newLoc.y] + 1;
                        }
                    }

                }

            }
        }
        return changed;
    }

    public void FlipRange(int range)
    {
        for (int x = 0; x < mapx; ++x)
        {
            for (int y =0; y < mapy; ++y)
            { 
                movemap[x, y] = Mathf.Abs(movemap[x, y] - range);
            }
        }
    }

    public void Combine()
    {
        for (int x = 0; x < mapx; ++x)
        {
            for (int y = 0; y < mapy; ++y)
            {
                movemap[x, y] *= threatmap[x, y];
            }
        }
    }

    public string debug()
    {
        string s = "";

        for (int y = 0; y < mapy; ++y)
        {
            for (int x = 0; x < mapx; ++x)
            {
                s = s + movemap[x, y].ToString() + "\t";
            }
            s = s + "\n";
        }
        return s;
    }
}
