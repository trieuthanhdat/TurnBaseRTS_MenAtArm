using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathFindingGridDebugObject : GridDebugObject
{
    
    [SerializeField] private TextMeshPro txtGCost;
    [SerializeField] private TextMeshPro txtHCost;
    [SerializeField] private TextMeshPro txtFCost;

    PathNode pathNode;
    public override void SetGridObject(object obj)
    {
        base.SetGridObject(obj);
        pathNode = (PathNode) obj;
    }
    public override void UpdateTextGridObject()
    {
        base.UpdateTextGridObject();
        txtGCost.text = pathNode.GetGCost().ToString();
        txtHCost.text = pathNode.GetHCost().ToString();
        txtFCost.text = pathNode.GetFCost().ToString();
    }
}
