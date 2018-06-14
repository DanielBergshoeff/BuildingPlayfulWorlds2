using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public float health;

    private float totalHealth;

    public GameObject healthBar;

	// Use this for initialization
	void Start () {
        totalHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health < 1)
        {
            gameObject.GetComponent<DiffuseScript>().StartDissolve(transform.position);
        }

        healthBar.GetComponent<Image>().fillAmount = health / totalHealth;
    }
}
