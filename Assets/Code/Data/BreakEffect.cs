using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Break")]
public class BreakEffect : Effect
{
    public int amount;
    public override void Apply(Character origin, Character target)
    {
        target.ChangeArmor(-amount);
    }
}
