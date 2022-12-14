using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding 
{
    private const int MOVE_COST = 10;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = MatrixManager.instance.pathNodesMatrix[startX, startY];
        PathNode endNode = MatrixManager.instance.pathNodesMatrix[endX, endY];

        /*Debug.Log("Start Node: " + startNode.x + ", " + startNode.y);
        Debug.Log("End Node: " + endNode.x + ", " + endNode.y);*/

        openList =  new List<PathNode> { startNode };
        closedList =  new List<PathNode>();

        for (int x = 0; x < MatrixManager.instance.pathNodesMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < MatrixManager.instance.pathNodesMatrix.GetLength(1); y++)
            {
                PathNode pathNode =  MatrixManager.instance.pathNodesMatrix[x, y];
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }

        }

        startNode.gCost = 0;
        startNode.hCost = Estimate_hCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {   
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //Final node reached
                //Debug.Log("End Node reached: " + endNode.x + ", " + endNode.y);
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Remove(currentNode);

            //Debug.Log("Before neighbour nodes loop");
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                int tentativeGCost = currentNode.gCost + Estimate_hCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = Estimate_hCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //Out of nodes in the open list
        //Debug.LogWarning("Pathfinding: Out of the open list");
        return null;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        //Debug.Log("Check neighbours of :" + currentNode.x + ", " + currentNode.y);

        if (currentNode.x - 1 >= 0)
        {//Left
            if (MatrixManager.instance.GetPjMovementMatrix()[currentNode.x - 1, currentNode.y])
            {
                neighbourList.Add(MatrixManager.instance.pathNodesMatrix[currentNode.x - 1, currentNode.y]);
            }
        }
        if (currentNode.y - 1 >= 0)
        {//Up
            if (MatrixManager.instance.GetPjMovementMatrix()[currentNode.x, currentNode.y - 1])
            {
                neighbourList.Add(MatrixManager.instance.pathNodesMatrix[currentNode.x, currentNode.y - 1]);
            }
        }
        if (currentNode.x + 1 < MatrixManager.instance.pathNodesMatrix.GetLength(0))
        {//Right
            if (MatrixManager.instance.GetPjMovementMatrix()[currentNode.x + 1, currentNode.y])
            {
                neighbourList.Add(MatrixManager.instance.pathNodesMatrix[currentNode.x + 1, currentNode.y]);
            }
        }
        if (currentNode.y + 1 < MatrixManager.instance.pathNodesMatrix.GetLength(1))
        {//Down
            if (MatrixManager.instance.GetPjMovementMatrix()[currentNode.x, currentNode.y + 1])
            {
                neighbourList.Add(MatrixManager.instance.pathNodesMatrix[currentNode.x, currentNode.y + 1]);
            }
        }

        return neighbourList;
    }

    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();

        return path;
    }

    private int Estimate_hCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        int crystalCost = 0;

        if(MatrixManager.instance.GetMechanicsLayoutMatrix()[b.x, b.y] >= 3 &&
           MatrixManager.instance.GetMechanicsLayoutMatrix()[b.x, b.y] <= 6)
           crystalCost += 10;
        if(MatrixManager.instance.GetMechanicsLayoutMatrix()[a.x, a.y] >= 3 &&
           MatrixManager.instance.GetMechanicsLayoutMatrix()[a.x, a.y] <= 6)
           crystalCost += 10;
        
        return ((xDistance + yDistance) * MOVE_COST) + crystalCost;

    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if(pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public int[] CalculatePathMovement(int startX, int startY, int endX, int endY)
    {
        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if(path == null)
        {
            //Debug.LogWarning("CalculatePath: Cannot find path");
            return null;
        }
        
        return FillMovementsFromPathNodes(path);
    }

    private int[] FillMovementsFromPathNodes(List<PathNode> path)
    {
        int[] pjMovementsArray = new int[0];
        foreach (PathNode node in path)
        {
            if(node.cameFromNode == null) continue;

            int [] oneMovement = new int[2] { node.x - node.cameFromNode.x, node.y - node.cameFromNode.y};
            if(oneMovement[1] == 1) pjMovementsArray = MatrixManager.instance.AddNumberToArray(pjMovementsArray, 1);
            else if(oneMovement[1] == -1) pjMovementsArray = MatrixManager.instance.AddNumberToArray(pjMovementsArray, -1);
            else if(oneMovement[0] == 1) pjMovementsArray = MatrixManager.instance.AddNumberToArray(pjMovementsArray, -2);
            else if(oneMovement[0] == -1) pjMovementsArray = MatrixManager.instance.AddNumberToArray(pjMovementsArray, 2);
        }

        return pjMovementsArray;
    }
}
