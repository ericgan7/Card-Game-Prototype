using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Double")]
public class DoubleEffect : Effect
{
    public Effect doubled;
    public override int Apply(Character origin, Character target)
    {
        foreach (Effect e in target.statusEffects)
        {
            if (e.GetType() == doubled.GetType())
            {
                e.StackEffect(e);
            }
        }
        return 0;
    }
}
