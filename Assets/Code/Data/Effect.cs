using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effect of a card when played. Should inherit from this class and implement Apply(), which is called when playing a card
///     Can potentiall implement on turn end, on turn start for debuff / buff skills that affect multiple rounds on a character.
/// </summary>
public class Effect : ScriptableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public virtual void Apply(Character origin, Character target)
    {

    }
}
