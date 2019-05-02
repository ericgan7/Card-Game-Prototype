using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/CardDraw")]
public class CardDrawEffect : Effect
{
    public int cardDraw;

    // Any effect that results in card draws
    public override void Apply(Character origin, Character target)
    {
        for(var i = 0; i < cardDraw; ++i)
        {
            target.DrawRandom();
        }
    }
}
