using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/DamageOnMove")]
public class DamageOnMoveEffect : Effect
{
    public int amount;
    public int duration;
    public override int Apply(Character origin, Character target)
    {
        target.statusEffects.Add(Instantiate(this));
        return 0;
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
}
