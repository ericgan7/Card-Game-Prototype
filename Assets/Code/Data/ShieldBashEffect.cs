using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/ShieldBash")]
public class ShieldBashEffect : Effect
{
    public float multiplier;
       
    // Effect that applies armor as part of the damage calculation
    public override int Apply(Character origin, Character target)
    {
        var damage = -(int)(origin.GetArmor() * multiplier);
        target.ChangeHealth(Mathf.Min(0, damage)+target.GetArmor());
        return damage;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, multiplier);
    }

    public override string GetAmount(Character origin, Character target)
    {
        return Mathf.Min(0, -(int)(origin.GetArmor() * multiplier) + target.GetArmor()).ToString();
    }
}
