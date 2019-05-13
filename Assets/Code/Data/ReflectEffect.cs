using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Reflect")]
public class ReflectEffect : Effect
{
    public int duration;
    public float multiplier;
    //potentially remove the armor buff and move it to its own effect
    public override int Apply(Character origin, Character target)
    {
        origin.statusEffects.Add(Instantiate(this));
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
        int damage = Mathf.Max(0, (int)(origin.GetDamage() * multiplier) - target.GetArmor());
        target.ChangeHealth(damage);
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, multiplier);
    }
}
