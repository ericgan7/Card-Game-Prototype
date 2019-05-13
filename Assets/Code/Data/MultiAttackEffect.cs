using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/MultiAttack")]
public class MultiAttackEffect : Effect
{
    public int attackCount;
    public float multiplier;
    public override int Apply(Character origin, Character target)
    {
        int damage = 0;
        for (var i = 0; i < attackCount; ++i)
        {
            damage += Mathf.Min(0, -(int)(origin.GetDamage() * multiplier) + target.GetArmor());
        }
        target.ChangeHealth(damage);
        return damage;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, origin.GetDamage(), attackCount);
    }

    public override string GetAmount(Character origin, Character target)
    {
        int damage = Mathf.Max(0, (int)(origin.GetDamage() * multiplier) - target.GetArmor());
        return (damage * attackCount).ToString();
    }
}
