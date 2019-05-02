using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/LoseEnergy")]
public class HealEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        var healAmount = target.ChangeHealth(target.GetHealth()[0] + amount);
        return healAmount;
    }
}
