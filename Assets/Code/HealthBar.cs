using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
	public Character character;
	public float health, currentHealth;
    public Image healthbar;
    public Image armorbar;
    public Image healthIcon;
    public Image armorIcon;
    public TextMeshProUGUI armortext;
    public TextMeshProUGUI healthtext;


    // Update is called once per frame
    void Update()
    {
    	if (character){
	    	Vector2Int h = character.GetHealth();
            health = h.y;
            currentHealth = h.x;
            float a = character.GetArmor();
            transform.position = character.transform.position;
            healthtext.text = h.x.ToString();
            armortext.text = a.ToString();
            healthbar.transform.localScale = new Vector3(currentHealth/health, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
            armorbar.transform.localScale = new Vector3(a / health, armorbar.transform.localScale.y, armorbar.transform.localScale.z);
            if (h.x < 1){
                Destroy(gameObject);
            }
        }
    }

    public void setCharacter(Character c){
    	character = c;
    }
}
