using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect of a card when played. Should inherit from this class and implement Apply(), which is called when playing a card
///     Can potentiall implement on turn end, on turn start for debuff / buff skills that affect multiple rounds on a character.
/// </summary>

public class Effect : ScriptableObject
{
    public Color color;
    //initial application of effect
    public virtual int Apply(Character origin, Character target) { return 0; }
    public virtual int Apply(Character origin, List<Vector3Int> ground) { return 0; }

    //triggers for effects that apply over multiple turns
    public virtual void OnTurnStart(Character origin) { }
    public virtual void OnTurnEnd(Character target) { }
    public virtual void OnMove(Character target) { }
    public virtual void OnAttack(Character origin, Character target) { }
    public virtual void OnDefend(Character origin, Character target) { }

    public virtual int ModifyAttack() { return 0; }
    public virtual int ModifySpeed() { return 0; }

    public virtual int GetScore(Character origin, Character target)
    {
        return 0;
    }
}