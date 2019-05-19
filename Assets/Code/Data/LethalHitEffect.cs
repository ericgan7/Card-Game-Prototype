using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/LethalHit")]
public class LethalHitEffect : Effect
{
    public float multiplier;
    public int energy;

    public override int Apply(Character origin, Character target)
    {
        int damage = Mathf.Min(0, -(int) (origin.GetDamage() * multiplier) + target.GetArmor());
        target.ChangeHealth(damage);
        if (target.GetHealth().x <= 0)
        {
            origin.ChangeEnergy(energy);
        }
        return damage;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, (int)(origin.GetDamage() * multiplier), energy);
    }

    public override string GetAmount(Character origin, Character target)
    {
        return Mathf.Min(0, -(int)(origin.GetDamage() * multiplier) + target.GetArmor()).ToString();
    }
}
