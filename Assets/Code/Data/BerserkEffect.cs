using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Berserk")]
public class BerserkEffect : BoostAttackEffect
{
    // Warrior-specific effect for gaining attack but losing hand size (need to reset handsize and attack somehow)
    public override void Apply(Character origin, Character target)
    {
        if(target.startingHand > 1)
        {
            base.Apply(origin, target);
            target.startingHand -= 1;
        }
    }
}
