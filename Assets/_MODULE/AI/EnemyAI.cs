using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        WaitingForEnemyTurn, 
        TakingTurn,
        Busy,
    }
    private State state;
    private const float MAX_ENEMY_TIMER = 2f;
    private float timer = MAX_ENEMY_TIMER;

    private void Start() 
    {
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if(!TurnSystem.instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = MAX_ENEMY_TIMER;
        }
    }

    private void Update() 
    {
        if(TurnSystem.instance.IsPlayerTurn()) return;
        ProcessState();
        if(timer <= 0)
        {
            state = State.TakingTurn;
            timer = MAX_ENEMY_TIMER;
        }
    }

    private void ProcessState()
    {
        switch(state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if(timer<= 0)
                {
                    state = State.Busy;
                    if(TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }else
                    {
                        //no more enemy taking action
                        TurnSystem.instance.UpdateTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }
    private void SetStateTakingTurn()
    {
        timer = 1f;
        state = State.TakingTurn;
    }
    private bool TryTakeEnemyAIAction(Action onEnemyAIActionCompleted)
    {
        foreach(UnitControl enemy in UnitManager.instance.GetEnemyUnits())
        {
            if(TryTakeEnemyAIAction(enemy, onEnemyAIActionCompleted))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(UnitControl enemy, Action onEnemyAIActionCompleted)
    {
        EnemyAIAction bestAIAction = null;
        BaseAction bestBaseAction = null;
        foreach(BaseAction baseAction in enemy.GetBaseActions())
        {
            if(!enemy.CanSpendActionPoint(baseAction))
                continue;
            if(bestAIAction == null)
            {
                bestAIAction = baseAction.GetBestAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                EnemyAIAction newBestAIAction = baseAction.GetBestAIAction();
                if(newBestAIAction != null && newBestAIAction.actionValue > bestAIAction.actionValue)
                {
                    bestAIAction = newBestAIAction;
                    bestBaseAction = baseAction;
                }
                Debug.Log("ENEMY AI: best AI Action "+ bestAIAction + " base action "+ bestBaseAction.GetActionName());

            }
        }
        if(bestAIAction != null && enemy.TrySpendActionPoint(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestAIAction.gridPosition, onEnemyAIActionCompleted);
            return true;
        }
        else
        {
            return false;
        }
    }
}
