using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/Draw")]
public class DrawCard : Effect
{
    public int amount;
    public override int Apply(Character origin, Character target)
    {
        origin.DrawCards(amount);
        return 0;
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount);
    }
}
