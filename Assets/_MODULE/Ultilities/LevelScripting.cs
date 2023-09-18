// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class LevelScripting : MonoBehaviour
// {

//     [SerializeField] private List<HiderTriggerObject> hiderTriggerObjects;
    
//     private bool hasShownFirstHider = false;

//     private void Start()
//     {
//         LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
//         foreach(var triggerObject in hiderTriggerObjects)
//         {
//             triggerObject.onTriggerPointEnter += (object sender, EventArgs e) =>
//             {
//                 SetActiveGameObjectList(hider1List, false);
//             };
//         }
//         foreach(var door in listDoor1)
//         {
//             door.OnDoorOpened += (object sender, EventArgs e) =>
//             {
//                 SetActiveGameObjectList(hider1List, false);
//             };
//         }
//         foreach(var door in listDoor2)
//         {
//             door.OnDoorOpened += (object sender, EventArgs e) =>
//             {
//                 SetActiveGameObjectList(hider3List, false);
//                 SetActiveGameObjectList(enemy2List, true);
//             };
//         }
//     }

//     private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, LevelGrid.OnAnyUnitMovedGridPositionEventArgs e)
//     {
//         if (e.toGridPosition.z == 5 && !hasShownFirstHider)
//         {
//             hasShownFirstHider = true;
//             SetActiveGameObjectList(hider1List, false);
//             SetActiveGameObjectList(enemy1List, true);
//         }
//     }

//     private void SetActiveGameObjectList(List<GameObject> gameObjectList, bool isActive)
//     {
//         foreach (GameObject gameObject in gameObjectList)
//         {
//             gameObject.SetActive(isActive);
//         }
//     }
// }
// public class HiderTriggerObject
// {
//     public HiderTriggerPoint hiderTriggerPoint;
   
// }
