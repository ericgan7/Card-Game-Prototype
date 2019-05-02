using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/BoostAttack")]
public class BoostAttackEffect : Effect
{
    public int increase;

    public override void Apply(Character origin, Character target)
    {
        target.ChangeDamage(target.GetDamage() + increase);
    }
}
