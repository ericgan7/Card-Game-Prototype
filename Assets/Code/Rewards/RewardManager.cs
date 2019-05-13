using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{ 
    public GM gm;
    public Deck deck;

    public Image[] icons;
    public CharacterStats[] characters;
    public RewardPool[] rewardPools;

    public Character[] chars;
    List<List<Reward>> rewards;
    public int index;

    public Choice[] choices;

    //stats
    public Image portrait;
    public TextMeshProUGUI charactername;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI end;
    public TextMeshProUGUI spd;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI arm;

    public void Start()
    {
        UpdateStats();
        rewards = new List<List<Reward>>();
        deck.SetCharacter(chars[index]);
        InitRewards();
        SetChoices();
    }

    void InitRewards()
    {
        for (int i = 0; i < characters.Length; ++i)
        {
            List<Reward> rw = new List<Reward>();
            int iteration = 0;
            while (rw.Count < 3)
            {
                ++iteration;
                if (iteration > 20)
                {
                    Debug.Log("generation error");
                    break;
                }
                Reward r = rewardPools[i].GenerateReward(0, 3);
                if (!rw.Contains(r))
                {
                    rw.Add(r);
                }
            }
            rewards.Add(rw);
        }
    }

    void SetChoices()
    {
        for(int i = 0; i < choices.Length; ++i)
        {
            choices[i].UpdateStat(rewards[index][i], chars[index]);
        }
    }

    public void EndReward()
    {
        gm.loadDialogue();
    }

    public void Scroll(int direction)
    {

    }

    public void MakeChoice(Choice c)
    {

    }

    public void UpdateStats()
    {
        portrait.sprite = characters[index].portrait;
        charactername.text = characters[index].characterName;
        hp.text = characters[index].health.ToString();
        end.text = characters[index].energy.ToString();
        spd.text = characters[index].speed.ToString();
        arm.text = characters[index].armor.ToString();
        dmg.text = characters[index].damage.ToString();
    }
}
