using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    Character selectedCharacter;

    public Image portrait;
    public TextMeshProUGUI charactername;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI end;
    public TextMeshProUGUI spd;
    public TextMeshProUGUI eva;
    public TextMeshProUGUI arm;

    public Image[] turnOrder;

    public void SelectCharacter(Character c)
    {
        selectedCharacter = c;
        UpdateStats();
    }

    public void UpdateStats()
    {
        portrait.sprite = selectedCharacter.stats.portrait;
        charactername.text = selectedCharacter.stats.characterName;
        Vector2Int h = selectedCharacter.GetHealth();
        hp.text = h.x.ToString() + " / " + h.y.ToString();
        h = selectedCharacter.GetEnergy();
        end.text = h.x.ToString() + " / " + h.y.ToString();
        spd.text = selectedCharacter.GetSpeed().ToString();
        eva.text = selectedCharacter.GetEvasion().ToString();
        arm.text = selectedCharacter.GetArmor().ToString();
    }

    public void UpdateTurns(List<Character> order)
    {
        Debug.Log("portrait");
        for (int i = 0; i < turnOrder.Length; ++i)
        {
            turnOrder[i].sprite = order[(i % order.Count)].stats.portrait;
        }
    }
}
