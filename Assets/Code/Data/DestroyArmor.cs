using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArmor : Effect
{
    public override int Apply(Character origin, Character target)
    {
        target.ChangeArmor(-target.GetArmor());
        return 0;
    }

    public override string GetAmount(Character origin, Character target)
    {
        return string.Format("-{0}", target.GetArmor());
    }
}
