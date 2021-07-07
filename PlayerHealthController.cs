using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int maxHealth, currentHealth;

    public float invincibleLength = 1f;
    private float inviCounter;
  

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (inviCounter > 0)
        {
            inviCounter -= Time.deltaTime;
        }
    }

    public void DamagePlayer(int damageAmount)
    {
        if(inviCounter <= 0 && !GameManager.instance.levelEnding)
        {
            AudioManager.instance.PlaySFX(4);
            UIController.instance.ShowDamage();
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                currentHealth = 0;
                GameManager.instance.PlayerDies();
                AudioManager.instance.StopBGM();
                AudioManager.instance.PlaySFX(3);
                AudioManager.instance.StopSFX(4);
            }
        }
        inviCounter = invincibleLength;

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }    

}
