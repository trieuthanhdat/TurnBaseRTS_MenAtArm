using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private const float MAX_ENEMY_TIMER = 2f;
    private float timer = MAX_ENEMY_TIMER;

    private void Start() 
    {
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        timer = MAX_ENEMY_TIMER;
    }

    private void Update() 
    {
        if(TurnSystem.instance.IsPlayerTurn()) return;

        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            TurnSystem.instance.UpdateTurn();
        }
    }

}
