using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/BoostAttack")]
public class BoostAttackEffect : Effect
{
    public int increase;

    public override int Apply(Character origin, Character target)
    {
        target.ChangeDamage(target.GetDamage() + increase);
        return increase;
    }
}
