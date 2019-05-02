using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/LethalHit")]
public class LethalHitEffect : Effect
{
    public float multiplier;
    public int armorPotency;

    public override int Apply(Character origin, Character target)
    {
        int damage = Mathf.Min(0, -(int)(origin.GetDamage() * multiplier) + target.GetArmor() * armorPotency);
        target.ChangeHealth(damage);
        return damage;
    }
}
