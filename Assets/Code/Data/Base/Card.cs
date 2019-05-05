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
    public enum RangeType
    {
        Area, Row, Cross
    }
    public enum TargetType
    {
        Self, Ally, Enemy, Ground
    }
    public EffectType etype;
    public RangeType rtype;
    public List<TargetType> targetsTypes;

    public int targetRange;
    public int effectRange;

    public struct EffectResult
    {
        public Sprite sprite;
        public string effect;
        public Color color;
        public Vector3 position;
    }

    virtual public List<EffectResult> Play(Character origin, List<Character> targets)
    {
        List<EffectResult> results = new List<EffectResult>();
        foreach (Character c in targets)
        {
            foreach( Effect e in effects)
            {
                int i = e.Apply(origin, c);
                EffectResult r = new EffectResult();
                r.effect = i.ToString();
                r.position = c.transform.position;
                r.color = e.color;
                results.Add(r);
            }
        }
        return results;
    }

    public List<EffectResult> Play(Character origin, List<Vector3Int> targets)
    {
        List<EffectResult> results = new List<EffectResult>();
        foreach (Effect e in effects)
        {
            int i = e.Apply(origin, targets);
        }

        return results;
    }
}
