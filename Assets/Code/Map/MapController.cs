using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Used as an interface for tilemap coloring and handles pathfinding on the grid.
/// </summary>

public class MapController : MonoBehaviour
{
    public GameController game;
    public Tilemap map;
    public HighlightTiles highlights;
    public HighlightTiles targets;

    Character[,] characterLocations;
    Dictionary<Character, Vector3Int> previousCharacterLocations;
    public int mapx;
    public int mapy;

    List<Vector3Int> frontier;
    Vector2Int[] neightbors;

    private void Start()
    {
        characterLocations = new Character[mapx, mapy];
        neightbors = new Vector2Int[4] { new Vector2Int(1,0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1)};
        frontier = new List<Vector3Int>();
        highlights = GetComponent<HighlightTiles>();
        previousCharacterLocations = new Dictionary<Character, Vector3Int>();
        InitCharacterMap();
    }

    //Converts to grid coordinates
    public Vector3Int WorldToCellSpace(Vector3 position)
    {
        //if need to modify by starting x or y position, do it here.
        Vector3Int l = map.WorldToCell(position);
        if (WithinMapBounds(l))
        {
            return l;
        }
        return new Vector3Int(-1, -1, -1);  //-1 represnts invalid worldspace location, grid starts with 0,0,0 as bottom left corner.
    }

    //Highlights the tilemap for movement and card range.
    public void Highlight(Vector3 pos, int area, HighlightTiles.TileType type, List<Card.TargetType> blockingTypes)
    {
        Vector3Int location = WorldToCellSpace(pos);
        highlights.FloodFill(location, area, type, blockingTypes);
    }
    //unhighlihgts a tile, currently used to make self an invalid target.
    public void UnHighlight(Vector3 pos)
    {
        Vector3Int location = WorldToCellSpace(pos);
        highlights.ChangeTile(location, HighlightTiles.TileType.None);
    }
    //clears out highlights.
    public void ClearHighlight()
    {
        highlights.Clear();
    }
    //highlights tiles when moused over a potential target in red.
    public void Target(Vector3 pos, Card.EffectType cardTag, int area, HighlightTiles.TileType type, List<Card.TargetType> blockingTypes)
    {
        Vector3Int origin = WorldToCellSpace(pos);
        if (highlights.Contains(origin)){
            switch (cardTag)
            {
                case Card.EffectType.Single:
                case Card.EffectType.Area:
                    targets.FloodFill(origin, area, type, blockingTypes);
                    break;
                case Card.EffectType.Chain:
                    Chain(origin, type);
                    break;
            }
        }
        else
        {
            targets.Clear();
        }
    }
    //clears out target highlights.
    public void ClearTarget()
    {
        targets.Clear();
    }
    //may be implemented properly in the furture, not important right now.
    void Chain(Vector3Int origin, HighlightTiles.TileType t)
    {
        HashSet<Vector3Int> explored = new HashSet<Vector3Int>();
        Queue<Vector3Int> frontier = new Queue<Vector3Int>();
        targets.ChangeTile(origin, t);
        if (characterLocations[origin.x, origin.y])
        {
            frontier.Enqueue(origin);
        }
        while (frontier.Count > 0)
        {
            Vector3Int current = frontier.Dequeue();
            if (characterLocations[origin.x, origin.y])
            {
                targets.ChangeTile(current, t);
                explored.Add(current);
                foreach(Vector3Int n in neightbors)
                {
                    Vector3Int next = n + current;
                    if (WithinMapBounds(next) && !explored.Contains(next))
                    {
                        frontier.Enqueue(next);
                    }
                }
            }
        }
    }

    //updates charactermap locations
    void InitCharacterMap()
    {
        foreach (Character e in game.enemies)
        {
            Vector3Int loc = WorldToCellSpace(e.transform.position);
            characterLocations[loc.x, loc.y] = e;
            previousCharacterLocations[e] = loc;
        }

        foreach(Character a in game.allies)
        {
            Vector3Int loc = WorldToCellSpace(a.transform.position);
            characterLocations[loc.x, loc.y] = a;
            previousCharacterLocations[a] = loc;
        }
    }
    //Gets the character at a specific location. can be null;
    public Character GetCharacter(Vector3Int location)
    {
        return characterLocations[location.x, location.y];
    }
    //A* pathfinding
    public List<Vector3Int> FindPath(Vector3Int origin, Vector3Int destination)
    {
        Debug.Log("START" + destination.ToString());
        frontier.Clear();
        Dictionary<Vector3Int, Vector3Int> paths = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, int> costs = new Dictionary<Vector3Int, int>();
        frontier.Add(new Vector3Int(origin.x, origin.y, 0));
        paths[origin] = origin;
        costs[origin] = 0;
        int iteration = 0;
        while(frontier.Count > 0)
        {
            Vector3Int current = PopFrontier();
            if (current.x == destination.x && current.y == destination.y)
            {
                break;
            }
            foreach(Vector2Int n in neightbors)
            {
                current.z = 0;
                int cost = costs[current] + 1;
                Vector3Int next = new Vector3Int(current.x + n.x, current.y + n.y, 0);
                if (WithinMapBounds(next) && (!costs.ContainsKey(next) || cost < costs[next]) && characterLocations[next.x, next.y] == null)
                {
                    costs[next] = cost;
                    InsertFrontier(new Vector3Int(next.x, next.y, cost + Mathf.Abs(destination.x - next.x) + Mathf.Abs(destination.y - next.y)));
                    paths[next] = current;
                }
            }
            ++iteration;
            if (iteration > 60)
            {
                Debug.Log("Pathfinding Failed");
                Debug.Log(paths.Count);
                break;
            }
        }
        List<Vector3Int> pathFound = new List<Vector3Int>();
        Vector3Int location = destination;
        iteration = 0;
        while(location != origin)
        {
            ++iteration;
            pathFound.Add(location);
            location = paths[location];
            if (iteration > 60)
            {
                Debug.Log("Pathbuilding Failed");
                Debug.Log(pathFound.Count);
            }
        }
        return pathFound;   //path return is in reverse order
    }
    //heap implementation, insertion
    private void InsertFrontier(Vector3Int newLocation)
    {
        int index = frontier.Count;
        int parent = (index - 1) / 2;
        frontier.Add(newLocation);
        while(index != 0 && frontier[index].z < frontier[parent].z)
        {
            Vector3Int t = frontier[parent];
            frontier[parent] = frontier[index];
            frontier[index] = t;
            index = parent;
            parent = (index - 1) / 2;
        }
    }
    //heap implementation, pop min element
    private Vector3Int PopFrontier()
    {
        Vector3Int min = frontier[0];
        if (frontier.Count > 1)
        {
            frontier[0] = frontier[frontier.Count - 1];
            frontier.RemoveAt(frontier.Count - 1);
            DecreaseFrontier(frontier[0], frontier[0].z);
        }
        else
        {
            frontier.RemoveAt(0);
        }
        return min;
    }
    //heap implementation, change key
    private void DecreaseFrontier(Vector3Int target, int cost)
    {
        int index = 0;
        for (int i = 0; i < frontier.Count; ++i)
        {
            if (frontier[i] == target)
            {
                index = i;
                break;
            }
        }
        target.z = cost;
        frontier[index] = target;
        int left = 2 * index + 1;
        int right = 2 * index + 2;
        bool larger = true;
        while (larger)
        {
            if (right < frontier.Count && frontier[index].z > frontier[right].z)
            {
                Vector3Int t = frontier[right];
                frontier[right] = frontier[index];
                frontier[index] = t;
                index = right;
                left = 2 * index + 1;
                right = 2 * index + 2;
            }
            else if (left < frontier.Count && frontier[index].z > frontier[left].z)
            {
                Vector3Int t = frontier[left];
                frontier[left] = frontier[index];
                frontier[index] = t;
                index = left;
                left = 2 * index + 1;
                right = 2 * index + 2;
            }
            else
            {
                larger = false;
            }
        }

    }
    //check within grid bounds
    public bool WithinMapBounds(Vector3Int location)
    {
        return location.x >= 0 && location.x < mapx && location.y >= 0 && location.y < mapy;
    }
    //TO DO: update character location maps during movement.
    public void UpdateCharacterMap(Character movedCharacter, Vector3Int newLocation)
    {
        Vector3Int previous = previousCharacterLocations[movedCharacter];
        characterLocations[previous.x, previous.y] = null;
        characterLocations[newLocation.x, newLocation.y] = movedCharacter;
        previousCharacterLocations[movedCharacter] = newLocation;

    }
}
