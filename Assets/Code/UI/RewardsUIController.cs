using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardsUIController : MonoBehaviour
{
    Character selectedCharacter;

    public Image portrait;
    public TextMeshProUGUI charactername;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI end;
    public TextMeshProUGUI spd;
    public TextMeshProUGUI eva;
    public TextMeshProUGUI arm;
    
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
    }

    public void FixedUpdate()
    {
        UpdateStats();
    }
}
