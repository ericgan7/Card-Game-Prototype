using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEffects : Effect
{
    public int amount;
    public int duration;
    public override int Apply(Character origin, Character target)
    {
        target.AddStatusEffect(Instantiate(this), true);
        return amount;
    }

    public override void StackEffect(Effect e)
    {
        MovementEffects t = (MovementEffects)e;
        amount += amount;
        duration += duration;
    }

    public override void OnTurnStart(Character origin)
    {
        --duration;
        if (duration <= 0)
        {
            origin.statusEffects.Remove(origin.statusEffects.Find(x => x.GetType() == typeof(MovementEffects)));
        }
    }

    public override int ModifySpeed()
    {
        return -amount;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount, duration);
    }

}
