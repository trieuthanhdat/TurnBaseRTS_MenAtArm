using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

public class PathFinding : MonoSingleton<PathFinding>
{
    [SerializeField] Transform gridDebugPrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private float cellSize = 2;
    private GridSystem<PathNode> gridSystem;
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14; //This is square root of 2
    private void Awake() 
    {
        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugPrefab);
        Debug.Log("PATH FINDING: int gridsystem width = "+gridSystem.Width + " height = "+gridSystem.Height);
    }
    //Testing
    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mousePos = LevelGrid.instance.GetGridPosition(MouseControl.GetPosition());
            GridPosition startPos = new GridPosition(0, 0);
            List<GridPosition> gridPositions = FindPaths(startPos, mousePos);

            for(int i = 0; i < gridPositions.Count - 1; i++)
            {
                Debug.DrawLine(LevelGrid.instance.GetWorldPosition(gridPositions[i]),
                               LevelGrid.instance.GetWorldPosition(gridPositions[i + 1]),
                               Color.green, 10f   );
            }
        }
    }
    public List<GridPosition> FindPaths(GridPosition startPos, GridPosition endPos)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = gridSystem.GetGridObject(startPos);
        PathNode endNode = gridSystem.GetGridObject(endPos);
        openList.Add(startNode);
        for(int x = 0; x < gridSystem.Width; x++)
        {
            for(int z = 0; z < gridSystem.Height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousNode();
            }
        }
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPos, endPos));
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            if(currentNode == endNode)
                return CalculatePathNode(endNode); //Reach final node
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighBour in GetNeightBourListPathNode(currentNode))
            {
                if(closedList.Contains(neighBour))
                {
                    continue;
                }

                int tGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighBour.GetGridPosition());

                if(tGCost < neighBour.GetGCost())
                {
                    neighBour.SetPreviousNode(currentNode);
                    neighBour.SetGCost(tGCost);
                    neighBour.SetHCost(CalculateDistance(neighBour.GetGridPosition(), endPos));
                    neighBour.CalculateFCost();

                    if(!openList.Contains(neighBour))
                    {
                        openList.Add(neighBour);
                    }
                }
            }
        }
        // no path found
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
            if(gridPosition.z - 1 >= 0)
                neightbours.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1)); //Left Bottom
            if(gridPosition.z + 1 < gridSystem.Height)
                neightbours.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1)); //Left Top
        }
        if(gridPosition.x + 1 < gridSystem.Width)
        {
            neightbours.Add(GetNode(gridPosition.x + 1, gridPosition.z)); //Right
            if(gridPosition.z - 1 >= 0)
                neightbours.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1)); //Right Bottom
            if(gridPosition.z + 1 < gridSystem.Height)
                neightbours.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1)); //Right Top
        }

        if(gridPosition.z - 1 >= 0)
            neightbours.Add(GetNode(gridPosition.x, gridPosition.z - 1)); //Bottom
        if(gridPosition.z + 1 < gridSystem.Height)
            neightbours.Add(GetNode(gridPosition.x, gridPosition.z + 1)); //Top
        
        return neightbours;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition distgrid = a - b;
        int dist = Mathf.Abs(distgrid.x) + Mathf.Abs(distgrid.z);
        int xDist = Mathf.Abs(distgrid.x);
        int zDist = Mathf.Abs(distgrid.z);
        int remaining = Mathf.Abs(distgrid.x - distgrid.z);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, zDist) + remaining * MOVE_STRAIGHT_COST;
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
}
