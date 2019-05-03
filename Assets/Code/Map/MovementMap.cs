using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMap : MonoBehaviour
{
    public int[,] movemap;
    MapController mc;
    Vector2Int[] neighbors;
    int mapx;
    int mapy;
    HashSet<Vector2Int> impasable;

    private void Start()
    {
        mc = GetComponent<MapController>();
        movemap = new int[mc.mapx, mc.mapy];
        neighbors = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
        mapx = mc.mapx;
        mapy = mc.mapy;
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
    public void UpdateMovementMap(int range = -1)
    {
        //reset maps;
        for (int x = 0; x < mapx; ++x)
        {
            for (int y = 0; y < mapy; ++y)
            {
                movemap[x, y] = 20;
            }
        }
        impasable.Clear();
        //assign goals
        List<Vector3Int> goals = mc.GetCharacterLocations();
        foreach (Vector3Int v in goals)
        {
            if (v.z == 0)
            {
                movemap[v.x, v.y] = range;
            }
            else
            {
                impasable.Add(new Vector2Int(v.x, v.y));
            }
        }
        //update maps until it stops changing
        bool changed = true;
        int iter = 0;
        while (changed)
        {
            changed = DijkstraMap();
            ++iter;
            if (iter > 20)
            {
                Debug.Log("DijkstraMap Error");
                Debug.Log(debug());
                break;
            }
        }
    }

    public bool DijkstraMap()
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
                        if (mc.WithinMapBounds((Vector3Int)newLoc) && movemap[newLoc.x, newLoc.y] + 1 < movemap[loc.x, loc.y])
                        {
                            changed = true;
                            movemap[loc.x, loc.y] = movemap[newLoc.x, newLoc.y] + 1;
                        }
                    }

                }

            }
        }
        return changed;
    }

    public string debug()
    {
        string s = "";

        for (int y = 0; y < mapy; ++y)
        {
            for (int x = 0; x < mapx; ++x)
            {
                s = s + movemap[x, y].ToString();
            }
            s = s + "\n";
        }
        return s;
    }
}
