using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour {

    public float health;
    public float maxHealth = 100f;
    //ZombieAI zombieAI;
    ZombieManager zombieManager;

	// Use this for initialization
	void Awake ()
    {
        zombieManager = GameObject.Find("EnemyManager").GetComponent<ZombieManager>();
        maxHealth = zombieManager.enemyHealth;
        health = maxHealth;
        
        //zombieAI = GetComponentInChildren<ZombieAI>();
        
	}

    public void TakeDamage(float _damage)
    {
        health -= _damage;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (health <= 0 )
        {
            zombieManager.EnemyCount--;
            DestroyObject(this.gameObject);
        }
        
	}
}
