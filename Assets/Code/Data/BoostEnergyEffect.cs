﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/BoostEnergy")]
public class BoostEnergyEffect : Effect
{
    public int amount;
    public override void Apply(Character origin, Character target)
    {
        target.ChangeEnergy(target.GetEnergy()[1] + 1);
    }
}
