using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Reflect")]
public class ReflectEffect : Effect
{
    public int duration;
    public Effect bleed;
    //potentially remove the armor buff and move it to its own effect
    public override int Apply(Character origin, Character target)
    {
        origin.AddStatusEffect(Instantiate(this), false);
        return 0;
    }

    //removes effect when expires
    public override void OnTurnStart(Character self)
    {
        duration -= 1;
        if (duration < 0)
        {
            self.statusEffects.Remove(this);
        }
    }

    public override void OnDefend(Character origin, Character target)
    {
        target.AddStatusEffect(bleed, false);
    }
}
