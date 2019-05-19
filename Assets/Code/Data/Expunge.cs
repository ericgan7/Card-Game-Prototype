using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Expunge")]
public class Expunge : Effect
{
    public DotStack stack;

    public override int Apply(Character origin, Character target)
    {
        int damage = 0;
        foreach(Effect e in target.statusEffects)
        {
            if (e.GetType() == stack.GetType())
            {
                DotStack s = (DotStack)e;
                damage = s.duration;
                break;
            }
        }
        target.ChangeHealth(-damage);
        return 0;
    }

    public override string GetAmount(Character origin, Character target)
    {
        int damage = 0;
        foreach (Effect e in target.statusEffects)
        {
            if (e.GetType() == stack.GetType())
            {
                DotStack s = (DotStack)e;
                damage = s.duration;
                break;
            }
        }
        return damage.ToString();
    }
}
