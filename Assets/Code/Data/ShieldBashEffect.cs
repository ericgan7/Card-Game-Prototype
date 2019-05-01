using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/ShieldBash")]
public class ShieldBasheffect : Effect
{
    public float multiplier;
       
    // Effect that applies armor as part of the damage calculation
    public override void Apply(Character origin, Character target)
    {
        var damage = -(int)((origin.GetDamage() + origin.GetArmor()) * multiplier);
        target.ChangeHealth(Mathf.Min(0, damage)+target.GetArmor());
    }
}
