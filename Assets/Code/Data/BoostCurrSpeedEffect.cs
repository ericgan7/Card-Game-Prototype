using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/BoostCurrSpeed")]
public class BoostCurrSpeedEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        target.ChangeEnergy(amount);
        return amount;
    }
}
