using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/DamageOverTime")]
public class DotStack: Effect
{
    public int duration;
    public override int Apply(Character origin, Character target)
    {
        target.AddStatusEffect(Instantiate(this), true);
        return 0;
    }

    public override void StackEffect(Effect e)
    {
        DotStack t = (DotStack)e;
        duration += t.duration;
        Debug.Log(duration);
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

    public override string ToString(Character origin)
    {
        return string.Format(description, duration);
    }
}
