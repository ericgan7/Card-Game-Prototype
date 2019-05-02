using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/RemoveDebuff")]
public class RemoveDebuffEffect : Effect
{
    public override int Apply(Character origin, Character target)
    {
        return 0;
    }
}
