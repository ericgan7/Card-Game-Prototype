using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Defend")]
public class DefendEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        target.ChangeArmor(amount);
        return amount;
    }

    public override int GetScore(Character origin, Character target)
    {
        return Mathf.Max(6 - origin.GetArmor(), 1);
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount);
    }

    public override string GetAmount(Character origin, Character target)
    {
        return amount.ToString();
    }
}
