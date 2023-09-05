using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] UnitControl unit =null;
    private MeshRenderer meshRenderer;
    private void Awake() 
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start() 
    {
        UnitAction.instance.OnSelectedUnit += UnityActionSystem_OnselectedUnitChanged;
        UpdateVisual();
    }

    private void UnityActionSystem_OnselectedUnitChanged(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitAction.instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
    private void OnDestroy()
    {
        UnitAction.instance.OnSelectedUnit -= UnityActionSystem_OnselectedUnitChanged;
    }
}
