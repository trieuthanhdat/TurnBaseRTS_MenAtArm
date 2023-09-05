using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnSystem : MonoSingleton<TurnSystem>
{
    private int currentTurn = 0;
    private bool isPlayerTurn = true;
    public event EventHandler OnTurnChange;
    public void UpdateTurn()
    {
        currentTurn ++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }
    public int GetTurnNumber()
    {
        return currentTurn;
    }
    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
