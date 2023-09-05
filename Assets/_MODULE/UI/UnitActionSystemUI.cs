using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionbtnPrefab;
    [SerializeField] private Transform actionbtnContainer;
    [SerializeField] private TextMeshProUGUI txtActionPoint;
    
    private List<ActionButtonUI> listButton = new List<ActionButtonUI>();
    void Start()
    {
        UnitAction.instance.OnSelectedUnit += UnitAction_OnSelectedUnit;
        UnitAction.instance.OnSelectedAction += UnitAction_OnSelectedAction;
        UnitAction.instance.onActionStarted += UnitAction_OnActionStarted;
        TurnSystem.instance.OnTurnChange += TurnSystem_OnTurnChange;
        UnitControl.OnAnyActionChange += Unit_OnAnyActionChange;
        CreateActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoint();
    }

    private void Unit_OnAnyActionChange(object sender, EventArgs e)
    {
        UpdateActionPoint();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateActionPoint();
    }

    private void UnitAction_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoint();
    }

    private void CreateActionButtons()
    {
        foreach(Transform child in actionbtnContainer)
        {
            Destroy(child.gameObject);
        }
        listButton.Clear();
        UnitControl selectedUnit = UnitAction.instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActions())
        {
            Transform btn = Instantiate(actionbtnPrefab, actionbtnContainer);
            ActionButtonUI actionButtonUI = btn.GetComponent<ActionButtonUI>();
            if(actionButtonUI != null)
            {
                actionButtonUI.SetBaseAction(baseAction);
                listButton.Add(actionButtonUI);
            }
        }
    }
    private void UnitAction_OnSelectedUnit(object sender, EventArgs e)
    {
        CreateActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoint();
    }
    private void UnitAction_OnSelectedAction(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        foreach (var btn in listButton)
        {
            btn.UpdateSelectedVisual();
        }
    }
    public void UpdateActionPoint()
    {
        UnitControl selectedUnit = UnitAction.instance.GetSelectedUnit();
        if(txtActionPoint)
        {
            txtActionPoint.text = "Action Point: "+ selectedUnit.GetActionPoint();
        }
    }
}
