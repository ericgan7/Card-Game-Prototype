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
        target.ChangeHealth(damage);
        return damage;
    }

    public override int GetScore(Character origin, Character target)
    {
        return (int)(origin.GetDamage() * multiplier) - target.GetArmor();
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, (origin.GetDamage() * multiplier)).ToString();
    }

    public override string GetAmount(Character origin, Character target)
    {
        return Mathf.Max(0, (int)(origin.GetDamage() * multiplier) - target.GetArmor()).ToString();
    }
}
