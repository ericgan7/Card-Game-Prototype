﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/BoostCurrSpeed")]
public class BoostCurrSpeedEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        target.ChangeSpeed(amount);
        return amount;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount);
    }
}
