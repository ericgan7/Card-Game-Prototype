using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Armorsteal")]
public class StealArmor : Effect
{
    public int steal;
    public override int Apply(Character origin, Character target)
    {
        origin.ChangeArmor(target.ChangeArmor(-steal));
        return 0;
    }

    public override string GetAmount(Character origin, Character target)
    {
        return steal.ToString();
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, steal);
    }
}
