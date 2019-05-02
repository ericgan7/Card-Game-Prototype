using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Defend")]
public class DefendEffect : Effect
{
    public int amount;
    public override EffectResult Apply(Character origin, Character target)
    {
        EffectResult result = new EffectResult();
        result.color = color;
        result.effect = amount.ToString();
        result.sprite = origin.stats.GetSprite("defend");
        target.ChangeArmor(amount);
        return result;
    }
}
