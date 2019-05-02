using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Break")]
public class BreakEffect : Effect
{
    public int amount;
    public override EffectResult Apply(Character origin, Character target)
    {
        EffectResult result = new EffectResult();
        result.color = color;
        result.effect= amount.ToString();
        result.sprite = target.stats.GetSprite("hurt");
        target.ChangeArmor(-amount);
        return result;

    }
}
