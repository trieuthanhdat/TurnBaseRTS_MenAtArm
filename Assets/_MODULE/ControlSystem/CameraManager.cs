using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject goActionCamera;
    [SerializeField] private float shoulderOffsetAmount = 0.5f;
    [SerializeField] private float camerCharacterHeightOffset = 1.7f;

    private void Start() 
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnActionCompleted += BaseAction_OnActionCompleted;
        HideActionCamera();
    }

    private void BaseAction_OnActionCompleted(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch(sender)
        {
            case ShootAction shootAction:
                UnitControl shooterUnit = shootAction.GetUnit();
                UnitControl targetUnit = shootAction.GetTargetUnit(); 
                Vector3 cameraCharacterHeight = Vector3.up * camerCharacterHeightOffset;
                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;
                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + shootDir * -1;

                goActionCamera.transform.position = actionCameraPosition;
                goActionCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                ShowActionCamera();
                break;
        }
    }

    private void ShowActionCamera()
    {
        goActionCamera.SetActive(true);
    }
    private void HideActionCamera()
    {
        goActionCamera.SetActive(false);
    }
}
