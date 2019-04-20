﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Defend")]
public class DefendEffect : Effect
{
    public int amount;
    public override void Apply(Character origin, Character target)
    {
        target.ChangeArmor(amount);
    }
}