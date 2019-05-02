using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/AdrenalineRush")]
public class AdrenalineRushEffect : AttackEffect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        var dmg = base.Apply(origin, target);
        if(target.GetHealth()[0] == 0)
        {
            target.ChangeEnergy(amount);
        }
        return dmg;
    }
}
