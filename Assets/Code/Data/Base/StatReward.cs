using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Reward", menuName = "Reward")]
public class StatReward : Reward
{
    public int healthBonus;
    public int energyBonus;
    public int speedBonus;
    public int damageBonus;
    public int armorBonus;

    public override string Stats()
    {
        int[] stats = new int[5] { healthBonus, energyBonus, speedBonus, damageBonus, armorBonus };
        string[] names = new string[5] { "Health", "Energy", "Speed", "Damage", "Armor" };
        string s = "";
        for (int i = 0; i < stats.Length; ++i)
        {
            if (stats[i] > 0)
            {
                s += string.Format("Gain {0} {1}. \n", stats[i], names[i]);
            }
        }
        return s;
    }
}
