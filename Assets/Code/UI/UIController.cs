using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI controller is used to mainintain ui elements of the turn inidcator and stats screen.
/// </summary>

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

    //when a new character is selected. Not currently used anywhere yet.
    public void SelectCharacter(Character c)
    {
        selectedCharacter = c;
        UpdateStats();
    }
    //Character stats panel;
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
    //updates the turn indicator, which shows which units will act.
    public void UpdateTurns(List<Character> order)
    {
        Debug.Log("portrait");
        for (int i = 0; i < turnOrder.Length; ++i)
        {
            turnOrder[i].sprite = order[(i % order.Count)].stats.portrait;
        }
    }
}
