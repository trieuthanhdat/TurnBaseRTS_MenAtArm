using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtActionPoint;
    [SerializeField] Image imgHealthBar;
    [SerializeField] UnitControl unit;
    [SerializeField] HealthSystem healthSystem;
    [SerializeField] Color enemyHealthColor = Color.red;
    [SerializeField] Color playerHealthColor = Color.green;


    private void Start() 
    {
        UnitControl.OnAnyActionChange += UnitControl_OnAnyActionPointChange;
        healthSystem.onDamage += HealthSystem_OnDamage;
        UpdateActionPointsText();
        UpdateHeathBar();
        if(unit.IsEnemy())
        {
            imgHealthBar.color = enemyHealthColor;
        }else
        {
            imgHealthBar.color = playerHealthColor;
        }
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e)
    {
        UpdateHeathBar();
    }

    private void UnitControl_OnAnyActionPointChange(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }
    private void UpdateHeathBar()
    {
        imgHealthBar.fillAmount = healthSystem.GetHealthProportion();
    }
    private void UpdateActionPointsText()
    {
        txtActionPoint.text = unit.GetActionPoint().ToString();
    }

}
