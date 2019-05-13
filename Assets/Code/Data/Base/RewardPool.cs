using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Reward", menuName = "Reward Pool")]
public class RewardPool : ScriptableObject
{
    public Reward[] pool;
    

    public Reward GenerateReward(int minimum, int maximum)
    {
        int min = Mathf.Clamp(minimum, 0, Mathf.Min(maximum, pool.Length));
        int max = Mathf.Clamp(maximum, min, Mathf.Min(maximum, pool.Length));
        int index = Random.Range(min, max);
        return pool[index];
    }
}
