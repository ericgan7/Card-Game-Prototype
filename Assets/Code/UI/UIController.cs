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

    public List<Icons> icons;
    public Icons iconPrefab;
    public GameObject statusEffects;

    public GameObject gameover;
    public GameObject pauseScreen;

    private void Start()
    {
        curve = GetComponent<BezierCurve>();
        displayText = GetComponent<DisplayText>();
        game = FindObjectOfType<GameController>();
        icons = new List<Icons>();
    }

    //when a new character is selected. Not currently used anywhere yet.
    public void SelectCharacter(Character c)
    {
        selectedCharacter = c;
        foreach (Icons i in icons)
        {
            Destroy(i.gameObject);
        }
        icons.Clear();
        for (int i = 0; i < selectedCharacter.statusEffects.Count; ++i)
        {
            Icons icon = Instantiate(iconPrefab);
            icon.SetEffect(selectedCharacter.statusEffects[i], selectedCharacter);
            icon.transform.SetParent(statusEffects.transform);
            icon.transform.localPosition = new Vector3(0f, i * 50f);
            icon.transform.localScale = Vector3.one;
            icons.Add(icon);
            Debug.Log("icon");
        }
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

    public void Gameover(bool over)
    {
        gameover.SetActive(over);
    }

    public void PauseGame(bool pause)
    {
        pauseScreen.SetActive(pause);
    }

}
