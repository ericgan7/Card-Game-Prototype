using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/ReduceAttack")]
public class ReduceAttackEffect : Effect
{
    public int amount;
    public int duration;
    public override int Apply(Character origin, Character target)
    {
        bool isApplied = false;
        foreach (Effect e in target.statusEffects)
        {
            if (e.GetType() == typeof(ReduceAttackEffect))
            {
                ReduceAttackEffect ra = (ReduceAttackEffect)e;
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
            origin.statusEffects.Remove(origin.statusEffects.Find(x => x.GetType() == typeof(ReduceAttackEffect)));
        }
    }

    public override int ModifyAttack()
    {
        return -amount;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount, duration);
    }

}
