using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Lifesteal")]
public class StealHealth : Effect
{
    public float steal;
    public override int Apply(Character origin, Character target)
    {
        int damage = Mathf.Max(0, origin.GetDamage() - target.GetArmor());
        target.ChangeHealth(-damage);
        origin.ChangeHealth(Mathf.CeilToInt(damage * steal));
        return 0;
    }

    public override string GetAmount(Character origin, Character target)
    {
        return (-Mathf.Max(0, origin.GetDamage() - target.GetArmor())).ToString();
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, (origin.GetDamage()).ToString());
    }
}
