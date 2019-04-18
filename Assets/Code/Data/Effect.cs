using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect of a card when played. Should inherit from this class and implement Apply(), which is called when playing a card
///     Can potentiall implement on turn end, on turn start for debuff / buff skills that affect multiple rounds on a character.
/// </summary>

public class Effect : ScriptableObject
{
    //initial application of effect
    public virtual void Apply(Character origin, Character target) { }

    //triggers for effects that apply over multiple turns
    public virtual void OnTurnStart(Character target) { }
    public virtual void OnTurnEnd(Character target) { }
    public virtual void OnMove(Character target) { }
}