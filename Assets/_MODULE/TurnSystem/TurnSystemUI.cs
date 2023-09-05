using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button btnEndTurn;
    [SerializeField] private TextMeshProUGUI txtTurnNumber;
    [SerializeField] private GameObject goEnemyTurn;
    // Start is called before the first frame update
    void Start()
    {
        if(btnEndTurn) btnEndTurn.onClick.AddListener(() => 
        {
            TurnSystem.instance.UpdateTurn();
        });
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
        UpdateNextTurnText();
        UpdateVisualEnemyTurn();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateNextTurnText();
        UpdateVisualEnemyTurn();
    }

    void UpdateNextTurnText()
    {
        if(txtTurnNumber) txtTurnNumber.text = "TURN "+TurnSystem.instance.GetTurnNumber();
    }
    void UpdateVisualEnemyTurn()
    {
        if(goEnemyTurn) goEnemyTurn.SetActive(!TurnSystem.instance.IsPlayerTurn());
    }
}
