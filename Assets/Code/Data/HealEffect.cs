using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Heal")]
public class HealEffect : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        var healAmount = target.ChangeHealth(amount);
        return healAmount;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount);
    }

    public override string GetAmount(Character origin, Character target)
    {
        Vector2Int hp = target.GetHealth();
        return Mathf.Min(hp.y - hp.x, amount).ToString();
    }
}
