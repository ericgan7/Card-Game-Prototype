﻿using System.Collections;
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

    public GameController game;

    public Image portrait;
    public TextMeshProUGUI charactername;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI end;
    public TextMeshProUGUI spd;
    public TextMeshProUGUI dmg;
    public TextMeshProUGUI arm;
    public TextMeshProUGUI energy;

    public Image[] turnOrder;
    public Image[] turnOrderBorderColor;

    public RadialMenu radialOptions;
    public ActionPanel ap;
    public BezierCurve curve;
    public DisplayText displayText;

    private void Start()
    {
        curve = GetComponent<BezierCurve>();
        displayText = GetComponent<DisplayText>();
        game = FindObjectOfType<GameController>();
    }

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
        arm.text = selectedCharacter.GetArmor().ToString();
        Vector2Int currentEnergy = game.currentCharacter.GetEnergy();
        energy.text = currentEnergy.x.ToString() + " / " + currentEnergy.y.ToString();
        dmg.text = selectedCharacter.GetDamage().ToString();
    }
    //updates the turn indicator, which shows which units will act.
    public void UpdateTurns(List<Character> order)
    {
        for (int i = 0; i < turnOrder.Length; ++i)
        {
            turnOrder[i].sprite = order[(i % order.Count)].stats.portrait;
            if (order[(i % order.Count)].team == Card.TargetType.Ally){
                turnOrderBorderColor[i].color = new Color(0, 1, 1, 1); //cyan
            }
            else if (order[(i % order.Count)].team == Card.TargetType.Enemy){
                turnOrderBorderColor[i].color = new Color(1, 0, 1, 1); //magenta
            }
        }
    }

    public void ActivateRadialMenu(Vector3 position)
    {
        radialOptions.gameObject.SetActive(true);
        radialOptions.ActivateMenu(null);
        radialOptions.transform.position = position;
    }

    public void DeactivateRadialMenu()
    {
        radialOptions.DeactiveMenu();
        radialOptions.gameObject.SetActive(false);
    }

    public void FixedUpdate()
    {
        UpdateStats();
    }

}
