using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect", menuName = "Effect/DelayedDamage")]
public class DelayedDamageEffect : Effect
{
    public int duration;
    public int amount;
    List<Vector3Int> targets;
    public override int Apply(Character origin, List<Vector3Int> ground)
    {
        DelayedDamageEffect copy = Instantiate(this);
        copy.targets = ground;
        origin.statusEffects.Add(copy);
        Debug.Log(origin.statusEffects.Count);
        return 0;
    }

    public override void OnTurnStart(Character origin)
    {
        --duration;
        if (duration <= 0)
        {
            GameController game = FindObjectOfType<GameController>();
            foreach (Vector3Int v in targets)
            {
                Character c = game.map.GetCharacter(v);
                if (c != null)
                {
                    c.ChangeHealth(-amount);
                }
            }
            origin.statusEffects.Remove(this);
        }
    }

    public override string ToString(Character origin)
    {
        return string.Format(description, amount, duration);
    }
}
