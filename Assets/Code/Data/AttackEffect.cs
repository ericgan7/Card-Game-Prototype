using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Attack")]
public class AttackEffect : Effect
{
    public float multiplier;

    public override EffectResult Apply(Character origin, Character target)
    {
        EffectResult result = new EffectResult();
        result.color = color;
        int damage = (Mathf.Min(0, -(int)(origin.GetDamage() * multiplier) + target.GetArmor()));
        result.effect = damage.ToString();
        result.sprite = target.stats.GetSprite("hurt");
        target.ChangeHealth(damage);
        return result;
    }
}
