using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiderTriggerPoint : MonoBehaviour
{
    public bool isTriggered = false;
    public List<Door> listDoorTrigger;
    public List<GameObject> hiderList;
    public List<GameObject> enemyList;
    public event EventHandler onTriggerPointEnter;
    private GridPosition gridPosition;
    private void Awake() 
    {
        gridPosition = LevelGrid.instance.GetGridPosition(transform.position);
    }
    private void Start() 
    {
        foreach(var door in listDoorTrigger)
        {
            door.OnDoorOpened += (object sender, EventArgs e) =>
            {
                ActivateTriggerPoint();
            };
        }
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
    {
        if(isTriggered) return;
        if(e.toGridPosition == gridPosition)
        {
            ActivateTriggerPoint();
        }
    }

    private void ActivateTriggerPoint()
    {
        SetActiveGameObjectList(hiderList, false);
        SetActiveGameObjectList(enemyList, true);
        isTriggered = true;
    }
    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("HIDER TRIGGER POINT: on trigger enter");
        if(isTriggered) return;
        if(other.gameObject.CompareTag("Player"))
        {
            ActivateTriggerPoint();
        }
    }
    private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            gameObject.SetActive(isActive);
        }
    }
}
