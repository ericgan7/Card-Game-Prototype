using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to hold Card data. Should contain all behavior, strings, art, cost, etc.
/// </summary>

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : Reward
{
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
        Self, Ally, Enemy, Ground, Obstacle
    }
    public EffectType etype;
    public RangeType rtype;
    public List<TargetType> highlightTypes;
    public List<TargetType> targetsTypes;

    public int targetRange;
    public int effectRange;

    public struct EffectResult
    {
        public string effect;
        public Color color;
        public Vector3 position;
    }
    Vector3 offset = new Vector3(0.1f, 0.2f, 0f);

    virtual public void Play(Character origin, List<Character> targets)
    {
        foreach (Character c in targets)
        {
            foreach( Effect e in effects)
            {
                int i = e.Apply(origin, c);
            }
        }
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

    public EnemyAI.CardScore GetScore(Character origin, List<Character> targets)
    {
        EnemyAI.CardScore best = new EnemyAI.CardScore
        {
            score = 0
        };
        foreach (Character t in targets)
        {
            int total = 0;
            if (targetsTypes.Contains(t.team)) {
                foreach (Effect e in effects)
                {
                    total += e.GetScore(origin, t);
                }
            }
            if (total > best.score)
            {
                best.score = total;
                best.target = t;
            }
        }
        return best;
    }

    public override string GetDescription(Character origin)
    {
        string description = "";
        foreach (Effect e in effects)
        {
            description += e.ToString(origin) + "\n";
        }
        return description;
    }

    public List<EffectResult> EffectAmount(Character origin, Character target)
    {
        List<EffectResult> results = new List<EffectResult>();
        foreach(Effect e in effects)
        {
            string amount = e.GetAmount(origin, target);
            if (amount != null)
            {
                results.Add(new EffectResult { color = e.color, effect = amount, position = target.transform.position + results.Count * offset});
            }
        }
        return results;
    }
}
