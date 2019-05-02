using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Attack")]
public class AttackEffect : Effect
{
    public float multiplier;

    public override int Apply(Character origin, Character target)
    {
        int damage = (Mathf.Min(0, -(int)(origin.GetDamage() * multiplier) + target.GetArmor()));
        return damage;
        target.ChangeHealth(damage);
    }
}
