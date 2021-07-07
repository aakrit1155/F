using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public int maxHealth, currentHealth;

    public Slider healthSlider;
    public Text healthText, ammoText;

    public Image damageEffect;
    public float damageAlpha = 0.5f, damageFadeSpeed = 0.5f;

    public Image blackScreen;
    public float fadeSpeed = 1.5f;

    public GameObject pauseScreen;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(damageEffect.color.a != 0)
        {
            damageEffect.color = new Color(255, 0, 0, Mathf.MoveTowards(damageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));
        }

        if(!GameManager.instance.levelEnding)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        }
    }

    public void ShowDamage()
    {
        damageEffect.color = new Color(255, 0, 0, .25f);
    }
}
