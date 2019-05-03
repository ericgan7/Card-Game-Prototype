using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Berserk")]
public class BerserkEffect : Effect
{
    // Warrior-specific effect for gaining attack but losing hand size (need to reset handsize and attack somehow)
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        if(target.startingHand > 1)
        {
            target.startingHand -= 1;
            target.ChangeDamage(target.GetDamage() + amount);
        }
        return 0;
    }
}
