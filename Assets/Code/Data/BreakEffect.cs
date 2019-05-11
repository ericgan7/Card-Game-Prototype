using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Break")]
public class BreakEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        target.ChangeArmor(-amount);
        return amount;

    }

    public override int GetScore(Character origin, Character target)
    {
        return target.GetArmor();
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount);
    }
}
