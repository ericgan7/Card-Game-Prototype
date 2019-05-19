using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Spawn")]
public class SpawnObstacle : Effect
{
    public Character prefab;

    public override int Apply(Character origin, List<Vector3Int> ground)
    {
        Character g = Instantiate(prefab);
        g.transform.position = ground[0];
        origin.game.AddObstacle(g, ground[0]);
        return 0;
    }
}
