using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/FlatDot")]
public class DotFlat : Effect
{
    public int duration;
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        target.AddStatusEffect(Instantiate(this), false);
        return 0;
    }

    public override void OnTurnStart(Character origin)
    {
        origin.ChangeHealth(-amount);
        --duration;
        if (duration <= 0)
        {
            origin.statusEffects.Remove(this);
        }
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount, duration);
    }
}
