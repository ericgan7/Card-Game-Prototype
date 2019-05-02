using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect of a card when played. Should inherit from this class and implement Apply(), which is called when playing a card
///     Can potentiall implement on turn end, on turn start for debuff / buff skills that affect multiple rounds on a character.
/// </summary>

public class Effect : ScriptableObject
{
    //TO DO : Change apply to return an result, which will give the sprite to render and text to display;
    public struct EffectResult
    {
        public Sprite sprite;
        public string effect;
        public Color color;
    }
    public Color color;
    //initial application of effect
    public virtual int Apply(Character origin, Character target) { return 0; }

    //triggers for effects that apply over multiple turns
    public virtual void OnTurnStart(Character target) { }
    public virtual void OnTurnEnd(Character target) { }
    public virtual void OnMove(Character target) { }
}