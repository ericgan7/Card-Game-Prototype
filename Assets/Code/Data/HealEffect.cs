using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Heal")]
public class HealEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        var healAmount = target.ChangeHealth(amount);
        return healAmount;
    }
}
