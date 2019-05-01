using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Enrage")]
public class EnrageEffect : BoostAttackEffect
{
    // Priest-specific, increases attack based on armor, and removes all armor
    public override void Apply(Character origin, Character target)
    {
        increase = target.GetArmor();
        target.ChangeArmor(0);
        base.Apply(origin, target);
    }
}
