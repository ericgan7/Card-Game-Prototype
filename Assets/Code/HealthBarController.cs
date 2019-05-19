using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
	public HealthBar healthBarPrefab;
	public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public HealthBar createHealthBar(Character c)
    {
    	HealthBar healthBar = Instantiate(healthBarPrefab);
        healthBar.transform.SetParent(transform.parent);
        healthBar.character = c;
        return healthBar;
    }
}
