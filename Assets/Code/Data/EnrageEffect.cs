using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Enrage")]
public class EnrageEffect : BoostAttackEffect
{
    // Priest-specific, increases attack based on armor, and removes all armor
    public override int Apply(Character origin, Character target)
    {
        int armor = target.GetArmor();
        target.ChangeArmor(-armor);
        target.ChangeDamage(armor);
        return armor;
    }
}
