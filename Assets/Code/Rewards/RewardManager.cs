using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{ 
    public GM gm;
    public Deck deck;
    public GameObject deckPos;

    public Image[] icons;
    public Image frame;
    Vector3 frameDestination;
    public float speed;
    public CharacterStats[] characters;
    public RewardPool[] rewardPools;

    public Character[] chars;
    List<List<Reward>> rewards;
    public int index;

    public Choice[] choices;
    public bool[] canChoose;

    //stats
    public Image portrait;
    public TextMeshProUGUI charactername;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI end;
    public TextMeshProUGUI spd;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI arm;

    public GameObject finished;

    public void Start()
    {
        gm = FindObjectOfType<GM>();
        characters = gm.characters;
        for (int i = 0; i < characters.Length; ++i)
        {
            chars[i].stats = characters[i];
            chars[i].SetStats();
            icons[i].sprite = characters[i].portrait;
        }
        UpdateStats();
        rewards = new List<List<Reward>>();
        deck.SetCharacter(chars[index]);
        InitRewards();
        frameDestination = frame.transform.localPosition;
        canChoose = new bool[chars.Length];
        for (int i = 0; i < canChoose.Length; ++i)
        {
            canChoose[i] = true;
        }
        SetChoices();
    }

    public void FixedUpdate()
    {
        frame.transform.localPosition = Vector3.Lerp(frame.transform.localPosition, frameDestination, Time.deltaTime * speed);
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
        if (canChoose[index])
        {
            for (int i = 0; i < choices.Length; ++i)
            {
                choices[i].gameObject.SetActive(true);
                choices[i].UpdateStat(rewards[index][i], chars[index]);
            }
        }
        else
        {
            for (int i = 0; i < choices.Length; ++i)
            {
                choices[i].gameObject.SetActive(false);
            }
        }
        bool done = true;
        foreach(bool c in canChoose)
        {
            if (c)
            {
                done = false;
                break;
            }
        }
        finished.SetActive(done);
    }

    public void EndReward()
    {
        gm.loadDialogue();
    }

    public void Scroll(int direction)
    {
        if (index == 0 && direction < 0)
        {
            index = characters.Length + direction;
        }
        else
        {
            index = (index + direction) % characters.Length;
        }
        UpdateStats();
        deck.SetCharacter(chars[index]);
        SetChoices();
        frameDestination = icons[index].transform.localPosition;
    }

    public void MakeChoice(Choice c)
    {
        canChoose[index] = false;
        c.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        characters[index].AddCard(c.reward);
        IEnumerator coroutine = AddCard(c);
        StartCoroutine(coroutine);
        SetChoices();
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

    IEnumerator AddCard(Choice chosen)
    {
        float elapsed = 0f;
        float total = 1f;
        Vector3 origin = chosen.transform.position;
        Vector3 size = chosen.transform.localScale;
        Choice c = Instantiate(chosen);
        c.activeChoice = false;
        c.gameObject.SetActive(true);
        c.transform.SetParent(chosen.transform.parent);
        c.scale = Vector3.one;
        while (elapsed < total)
        {
            c.transform.position = Vector3.Lerp(origin, deckPos.transform.position, elapsed / total);
            c.transform.localScale = Vector3.Lerp(size, Vector3.one, elapsed / total);
            elapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(c.gameObject);
    }
}
