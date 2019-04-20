using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Attack")]
public class AttackEffect : Effect
{
    public float multiplier;

    public override void Apply(Character origin, Character target)
    {
        target.ChangeHealth((Mathf.Min(0, -(int)(origin.GetDamage() * multiplier)-target.GetArmor())));
    }
}
