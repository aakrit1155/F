using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private bool isCollected;
    public int healAmount;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !isCollected)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(2);
        }
    }
}
