using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to hold Card data. Should contain all behavior, strings, art, cost, etc.
/// </summary>

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite art;
    public int energyCost;

    public Effect[] effects;
    public string spriteName;
    public enum EffectType
    {
        Targetable, Nontargetable
    }
    public enum TargetType
    {
        Self, Ally, Enemy, Ground
    }
    public EffectType etype;
    public List<TargetType> targetsTypes;

    public int targetRange;
    public int effectRange;

    virtual public List<Effect.EffectResult> Play(Character origin, List<Character> targets)
    {
        List<Effect.EffectResult> results = new List<Effect.EffectResult>();
        Effect.EffectResult r = new Effect.EffectResult();
        r.sprite = origin.stats.GetSprite(spriteName);
        results.Add(r);
        foreach (Character c in targets)
        {
            foreach( Effect e in effects)
            {
                Effect.EffectResult result = e.Apply(origin, c);
                if (result.effect != null)
                {
                    results.Add(result);
                }
            }
        }
        return results;
    }
}
