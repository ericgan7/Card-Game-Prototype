using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	public Character character;
	public float health, currentHealth;
    public Transform mask;
    public Transform image;

    // Start is called before the first frame update
    void Start()
    {
    	mask = transform.GetChild(0);
        image = mask.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
    	if (character){
	    	Vector2Int h = character.GetHealth();
	    	health = h.y;
	    	currentHealth = h.x;
            transform.position = character.transform.position + new Vector3(-1.6f, 0.7f, 0);
	    	image.transform.localScale = new Vector3(currentHealth/health, image.transform.localScale.y, image.transform.localScale.z);
	        Debug.Log(h.x);
            if (h.x < 1){
                Destroy(gameObject);
            }
        }
    }

    public void setCharacter(Character c){
    	character = c;
    }
}
