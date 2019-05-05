using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEffects : Effect
{
    public int amount;
    public int duration;
    public override int Apply(Character origin, Character target)
    {
        bool isApplied = false;
        foreach (Effect e in target.statusEffects)
        {
            if (e.GetType() == typeof(MovementEffects))
            {
                MovementEffects ra = (MovementEffects)e;
                ra.duration += duration;
                ra.amount = Mathf.Max(ra.amount, amount);
                isApplied = true;
                break;
            }
        }
        if (!isApplied)
        {
            target.statusEffects.Add(Instantiate(this));
        }
        return amount;
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
}
