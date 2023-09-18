using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private GridPosition gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private bool canWalk = true;

    private PathNode previousPathNode;
    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }
    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void CalculateFCost()
    {
        this.fCost = gCost + hCost;
    }
    public int GetGCost()
    {
        return gCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetFCost()
    {
        return fCost;
    }
    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }
    public void ResetPreviousNode()
    {
        previousPathNode = null;
    }

    public void SetPreviousNode(PathNode node)
    {
        previousPathNode = node;
    }
    public PathNode GetPreviousNode()
    {
        return previousPathNode;
    }
    public bool Walkable()
    {
        return canWalk;
    }
    public void SetWalkable(bool canWalk)
    {
        this.canWalk = canWalk;
    }
}
