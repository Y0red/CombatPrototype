using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHealthBar : MonoBehaviour
{
    Vector3 localScale;
    CharacterStats enemyStats;
    void Start()
    {
        enemyStats = transform.parent.GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStats != null)
        {
            localScale.x = (float)enemyStats.characterDefination.currentHealth / enemyStats.characterDefination.maxHealth;
            localScale.y = 1f;
          
            transform.localScale = localScale;
        }
        transform.LookAt(Camera.main.transform);
    }
}
