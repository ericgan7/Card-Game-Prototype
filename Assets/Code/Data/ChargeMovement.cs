using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Attack")]
public class ChargeMovement : Effect
{
    public override int Apply(Character origin, Character target)
    {
        Vector3Int o = origin.GetPosition();
        Vector3Int t = target.GetPosition();
        Vector3Int mod;
        Vector3Int distance = new Vector3Int(t.x - o.x, t.y - o.y, 0);
        if (distance.y == 0)
        {
            if (distance.x > 0)
            {
                mod = new Vector3Int(-1, 0, 0);
            }
            else
            {
                mod = new Vector3Int(1, 0, 0);
            }
        }
        else
        {
            if (distance.y > 0)
            {
                mod = new Vector3Int(0, -1, 0);
            }
            else
            {
                mod = new Vector3Int(0, 1, 0);
            }
        }
        Debug.Log(distance);
        origin.destinations.Add(o + distance + mod);
        origin.Move(0);
        int damage = distance.x + distance.y + mod.x + mod.y + origin.GetDamage();
        target.ChangeHealth(-Mathf.Max(0, damage - target.GetArmor()));
        return 0;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, origin.GetDamage());
    }

    public override string GetAmount(Character origin, Character target)
    {
        Vector3Int o = origin.GetPosition();
        Vector3Int t = origin.GetPosition();
        int distance = t.x - o.x + t.y - o.y - 1;
        return Mathf.Max(0, distance + origin.GetDamage() - target.GetArmor()).ToString();
    }
}
