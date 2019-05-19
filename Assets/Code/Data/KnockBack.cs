using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Knockback")]
public class KnockBack : Effect
{
    public int push;
    public override int Apply(Character origin, Character target)
    {
        Vector3Int t = target.GetPosition();
        Vector3Int o = origin.GetPosition();
        Vector3 direction = Vector3.Normalize(t - o);
        Vector3Int tile = new Vector3Int((int)direction.x, (int)direction.y, 0);
        Vector3Int destination = t + tile * push;
        for (int i = 0; i < push; ++i)
        {
            if (origin.game.map.GetCharacter(destination - tile* i)== null)
            {
                target.destinations.Add(destination - tile * i);
                target.Move(0);
                break;
            }
        }
        return 0;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, push);
    }

}
