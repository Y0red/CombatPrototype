using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Manager<GameManager>
{
    public GameObject[] SystemPrefabs;

    public GameState gameState;
    // public Events.EventGameState OnGameStateChanged;

    public string currentLevelName = string.Empty;

    private HeroMovement heroController;

    private HeroMovement hero
    {
        get
        {
            if(null == heroController)
            {
                heroController = FindObjectOfType<HeroMovement>();
            }
            return heroController;
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region CallBacks
    public void OnHeroLeveledUp(int arg0)
    {
        UiManager.Instance.UpdateUnitFrame(hero);
        SoundManager.Instance.PlaySoundEffect(SoundEffects.LevelUp);
    }

    public void OnHeroDamaged(int damage)
    {
        UiManager.Instance.UpdateUnitFrame(hero);
        SoundManager.Instance.PlaySoundEffect(SoundEffects.HeroHit);
    }
    public void OnHeroGainedHealth(int health)
    {
        UiManager.Instance.UpdateUnitFrame(hero);
        SoundManager.Instance.PlaySoundEffect(SoundEffects.LevelUp);
    }
    public void OnHeroDied()
    {
        UiManager.Instance.UpdateUnitFrame(hero);
        SoundManager.Instance.PlaySoundEffect(SoundEffects.HeroHit);
    }
    public void OnHeroInit()
    {
        UiManager.Instance.InitUnitFrame();
        UiManager.Instance.UpdateUnitFrame(hero);
        Debug.LogWarning("Hero initialized");
    }


    #endregion

    public class GameState
    {
    }
}

