using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Pierce")]
public class PierceEffect : Effect
{
    public float multiplier;

    // Attack except you ignore the armor
    public override void Apply(Character origin, Character target)
    {
        target.ChangeHealth(Mathf.Min(0, -(int)(origin.GetDamage() * multiplier)));
    }
}
