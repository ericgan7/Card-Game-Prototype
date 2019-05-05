using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/DamageOverTime")]
public class DamageOverTimeEffect : Effect
{
    public int duration;
    public override int Apply(Character origin, Character target)
    {
        bool isApplied = false;
        foreach (Effect e in target.statusEffects)
        {
            if (e.GetType() == typeof(DamageOverTimeEffect))
            {
                DamageOverTimeEffect dot = (DamageOverTimeEffect)e;
                dot.duration += duration;
                isApplied = true;
                break;
            }
        }
        if (!isApplied)
        {
            target.statusEffects.Add(Instantiate(this));
        }
        return 0;
    }

    public override void OnTurnStart(Character origin)
    {
        origin.ChangeHealth(-duration);
        --duration;
        if (duration <= 0)
        {
            origin.statusEffects.Remove(this);
        }
    }
}
