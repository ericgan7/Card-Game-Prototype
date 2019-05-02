using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/BoostEnergy")]
public class BoostMaxEnergyEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        target.ChangeEnergy(target.GetEnergy()[1] + amount);
        return 0;
    }
}
