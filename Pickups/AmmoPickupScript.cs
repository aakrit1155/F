using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickupScript : MonoBehaviour
{
    private bool collected= false;
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player" && !collected)
        {
            //give ammo 
            PlayerController.instance.activeGun.GetAmmo();    
            Destroy(gameObject);
            collected = true;

            AudioManager.instance.PlaySFX(7);
        }
    }
}
