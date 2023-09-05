using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtAction;
    [SerializeField] private Button button;
    [SerializeField] private Image imgAction;
    [SerializeField] private GameObject goSelected;
    private BaseAction baseAction;

    public void SetBaseAction(BaseAction action)
    {
        this.baseAction = action;
        txtAction.text = action.GetActionName().ToUpper();
        button.onClick.AddListener(delegate {OnBtnClickAction(action);});
        imgAction.sprite = action.GetActionSprite();
    }

    private void OnBtnClickAction(BaseAction action)
    {
        UnitAction.instance.SelecteAction(action);
    }
    public void UpdateSelectedVisual()
    {
        BaseAction selectedAction = UnitAction.instance.GetSelectedAction();
        goSelected.SetActive(selectedAction == baseAction);
    }
    
}
