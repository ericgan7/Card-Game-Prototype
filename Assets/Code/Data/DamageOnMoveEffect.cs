﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/DamageOnMove")]
public class DamageOnMoveEffect : Effect
{
    public int amount;
    public int duration;
    public override int Apply(Character origin, Character target)
    {
        target.AddStatusEffect(Instantiate(this), true);
        return 0;
    }

    //Currently does not stack damage, only refreshes
    public override void StackEffect(Effect e)
    {
        DamageOnMoveEffect t = (DamageOnMoveEffect)e;
        duration += t.duration;
    }

    public override void OnTurnStart(Character origin)
    {
        --duration;
        if (duration < 0)
        {
            origin.statusEffects.Remove(this);
        }
    }

    public override void OnMove(Character target)
    {
        target.ChangeHealth(-amount);
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount, duration);
    }
}
