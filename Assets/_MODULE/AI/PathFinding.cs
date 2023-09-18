using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class PathFinding : MonoSingleton<PathFinding>
{
    [SerializeField] Transform gridDebugPrefab;
    [SerializeField] LayerMask obstacleLayerMask;
    private int width = 10;
    private int height = 10;
    private float cellSize = 2;
    private GridSystemHex<PathNode> GridSystemHex;
    private const int MOVE_STRAIGHT_COST = 10;

    public int GetMoveStaightCost()
    {
        return MOVE_STRAIGHT_COST;
    }
    public void Setup(int w, int h, float cellSize)
    {
        this.width = w;
        this.height = h;
        this.cellSize = cellSize;
        GridSystemHex = new GridSystemHex<PathNode>(width, height, cellSize, (GridSystemHex<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        GridSystemHex.CreateDebugObjects(gridDebugPrefab);
        Debug.Log("PATH FINDING: int GridSystemHex width = "+GridSystemHex.Width + " height = "+GridSystemHex.Height);

        //Applying offsets 
        for(int x = 0; x< width; x ++)
        {
            for(int z = 0; z < height; z ++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPos = LevelGrid.instance.GetWorldPosition(gridPosition);
                float raycastOffset = 5f;
                if(Physics.Raycast(worldPos + Vector3.down * raycastOffset, Vector3.up, raycastOffset * 2, obstacleLayerMask))
                {
                    GetNode(x, z).SetWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPaths(GridPosition startPos, GridPosition endPos, out int pathLength)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = GridSystemHex.GetGridObject(startPos);
        PathNode endNode = GridSystemHex.GetGridObject(endPos);
        openList.Add(startNode);
        for(int x = 0; x < GridSystemHex.Width; x++)
        {
            for(int z = 0; z < GridSystemHex.Height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = GridSystemHex.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousNode();
            }
        }
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateHexDistance(startPos, endPos));
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if(currentNode == endNode)
            {
                pathLength = endNode.GetFCost();
                return CalculatePathNode(endNode); //Reach final node
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighBour in GetNeightBourListPathNode(currentNode))
            {
                if(closedList.Contains(neighBour))
                {
                    continue;
                }
                if(!neighBour.Walkable())
                {
                    closedList.Add(neighBour);
                    continue;
                }
                int tGCost = currentNode.GetGCost() + MOVE_STRAIGHT_COST;

                if(tGCost < neighBour.GetGCost())
                {
                    neighBour.SetPreviousNode(currentNode);
                    neighBour.SetGCost(tGCost);
                    neighBour.SetHCost(CalculateHexDistance(neighBour.GetGridPosition(), endPos));
                    neighBour.CalculateFCost();

                    if(!openList.Contains(neighBour))
                    {
                        openList.Add(neighBour);
                    }
                }
            }
        }
        // no path found
        pathLength = 0;
        return null;
    }

    private List<GridPosition> CalculatePathNode(PathNode endNode)
    {
        List<PathNode> listPathNode = new List<PathNode>();
        listPathNode.Add(endNode);
        PathNode currNode = endNode;

        while(currNode.GetPreviousNode() != null)
        {
            listPathNode.Add(currNode.GetPreviousNode());
            currNode = currNode.GetPreviousNode();
        }
        listPathNode.Reverse(); // Reverse the list
        List<GridPosition> gridPositions = new List<GridPosition>();
        foreach(var node in listPathNode)
        {
            gridPositions.Add(node.GetGridPosition());
        }
        return gridPositions;
    }
    private List<PathNode> GetNeightBourListPathNode(PathNode currNode)
    {
        List<PathNode> neightbours = new List<PathNode>();

        GridPosition gridPosition = currNode.GetGridPosition();
        if(gridPosition.x - 1 >= 0)
        {
            neightbours.Add(GetNode(gridPosition.x - 1, gridPosition.z)); //Left
            
        }
        if(gridPosition.x + 1 < GridSystemHex.Width)
        {
            neightbours.Add(GetNode(gridPosition.x + 1, gridPosition.z)); //Right
        }

        if(gridPosition.z - 1 >= 0)
            neightbours.Add(GetNode(gridPosition.x, gridPosition.z - 1)); //Bottom
        if(gridPosition.z + 1 < GridSystemHex.Height)
            neightbours.Add(GetNode(gridPosition.x, gridPosition.z + 1)); //Top
        
        bool oddRow = gridPosition.z % 2 == 1;
        if(oddRow)
        {
            if(gridPosition.x + 1 < GridSystemHex.Width)
            {
                if(gridPosition.z - 1 >= 0)
                    neightbours.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1)); //Right bot
                if(gridPosition.z + 1 < GridSystemHex.Height)
                    neightbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1)); //Right top
            }
        }else
        {
            if(gridPosition.x - 1 >= 0)
            {
                if(gridPosition.z - 1 >= 0)
                    neightbours.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1)); //Left bot
                if(gridPosition.z + 1 < GridSystemHex.Height)
                    neightbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1)); //Left top
            }
           
        }
        return neightbours;
    }

    private PathNode GetNode(int x, int z)
    {
        return GridSystemHex.GetGridObject(new GridPosition(x, z));
    }

    public int CalculateHexDistance(GridPosition a, GridPosition b)
    {
        return Mathf.RoundToInt(Vector3.Distance(GridSystemHex.GetWorldPosition(a), GridSystemHex.GetWorldPosition(b)) * MOVE_STRAIGHT_COST);
        // GridPosition distgrid = a - b;
        // int xDist = Mathf.Abs(distgrid.x);
        // int zDist = Mathf.Abs(distgrid.z);
        // int remaining = Mathf.Abs(distgrid.x - distgrid.z);
        // return MOVE_STRAIGHT_COST * Mathf.Min(xDist, zDist) + remaining * MOVE_STRAIGHT_COST;
    }
    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodes)
    {
        PathNode lowestNode = pathNodes[0];
        for(int i = 0; i< pathNodes.Count; i++)
        {
            if(pathNodes[i].GetFCost() < lowestNode.GetFCost())
            {
                lowestNode = pathNodes[i];
            }
        }
        return lowestNode;
    }
    public bool IsGridPositionWalkable(GridPosition gridPosition)
    {
        return GridSystemHex.GetGridObject(gridPosition).Walkable();
    }
    public void SetGridPositionWalkable(GridPosition gridPosition, bool isWalkable)
    {
        GridSystemHex.GetGridObject(gridPosition).SetWalkable(isWalkable);
    }
    public void RemoveTargetAtGridPosition()
    {
        
    }
    public bool HasValidPath(GridPosition startPosition, GridPosition endPosition)
    {
        return FindPaths(startPosition, endPosition, out int pathLength) != null;
    }
    public bool ValidatePathsAndLength(GridPosition startPosition, GridPosition endPosition, int validateLength)
    {
        bool isValid = false;
        if(FindPaths(startPosition, endPosition, out int pathLength) != null)
        {
            if(pathLength <= validateLength)
            {
                Debug.Log("VALIDATE PATHS: pathlength "+pathLength + " validate legnth "+validateLength);
                isValid = true;
            }
        }
        return isValid;
    }
    public int GetPathLength(GridPosition startPosition, GridPosition endPosition)
    {
        FindPaths(startPosition, endPosition, out int pathLength);
        return pathLength;
    }
}
