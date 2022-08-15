using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : Manager<UiManager>
{
    [SerializeField] private GameObject unitFrame;

    [SerializeField] private Image healthBar;
    [SerializeField] private Text levelText;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitUnitFrame()
    {
        levelText.text = "1";
        healthBar.fillAmount = 1;
    }
    public void UpdateUnitFrame(HeroMovement hero)
    {
        int currHealth = hero.GetCurrentHealth();
        int maxHealth = hero.GetMaxHealth();

        healthBar.fillAmount = (float)currHealth / maxHealth;
        levelText.text = hero.GetCurrentLevel().ToString();
    }
}
