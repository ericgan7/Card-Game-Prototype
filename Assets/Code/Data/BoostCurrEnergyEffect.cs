using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/BoostCurrEnergy")]
public class BoostCurrEnergyEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        var changeAmount = target.ChangeEnergy(target.GetEnergy()[0] + amount);
        return changeAmount;
    }
}
