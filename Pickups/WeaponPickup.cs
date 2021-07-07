using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public string theGun;
    private bool collected;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !collected)
        {
            //give ammo 
            PlayerController.instance.AddGun(theGun);
            Destroy(gameObject);
            collected = true;
            AudioManager.instance.PlaySFX(6);
        }
    }
}
