using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public Weapon weapon;

    AudioSource source;

    void Start()
    {
        source = GameObject.FindGameObjectWithTag("UIManager").GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int playerID = other.transform.root.gameObject.GetComponent<PlayerController>().playerID;
            UIManager uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
            uiManager.StopTextLerp();

            if (source)
                source.Play();

            other.transform.root.GetComponent<Shooting>().UpdateWeapon(weapon);
            uiManager.UpdateAmmo(playerID);
            uiManager.DisplayWeaponText(playerID, weapon.name, weapon.itemActivator);

            if (GetComponentInParent<ItemSpawn>())
                GetComponentInParent<ItemSpawn>().hasItem = false;

            Destroy(gameObject);
        }
    }
}
