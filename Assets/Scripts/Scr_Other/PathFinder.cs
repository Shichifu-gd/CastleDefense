using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private Transform Target;

    public LayerMask Barriers;
    public LayerMask Cell;

    private List<Vector2> PathToTarget = new List<Vector2>();
    private List<Node> CheckedNodes = new List<Node>();
    private List<Node> WaitingNodes = new List<Node>();

    public List<Vector2> GetPath(Transform target)
    {
        Target = target;
        NullifyLists();
        Vector2 startPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 targetPosition = new Vector2(Mathf.Round(Target.transform.position.x), Mathf.Round(Target.transform.position.y));
        if (startPosition == targetPosition) return PathToTarget;
        Node startNode = new Node(0, startPosition, targetPosition, null);
        CheckedNodes.Add(startNode);
        WaitingNodes.AddRange(FindNeighboringNodes(startNode));
        while (WaitingNodes.Count > 0)
        {
            Node nodeToCheck = WaitingNodes.Where(x => x.Cost == WaitingNodes.Min(y => y.Cost)).FirstOrDefault();
            if (nodeToCheck.CurrentPosition == targetPosition)
            {
                // TODO: при завершении тестов, заменить возврат и убрать OnDrawGizmos.
                PathToTarget = CalculatePathFromNode(nodeToCheck);
                return PathToTarget;
            }
            var cell = Physics2D.OverlapCircle(nodeToCheck.CurrentPosition, 0.1f, Cell);
            if (cell)
            {
                var passableСell = !Physics2D.OverlapCircle(nodeToCheck.CurrentPosition, 0.1f, Barriers);
                if (!passableСell) AddToListCheckedNodes(nodeToCheck);
                else if (passableСell)
                {
                    WaitingNodes.Remove(nodeToCheck);
                    if (!CheckedNodes.Where(x => x.CurrentPosition == nodeToCheck.CurrentPosition).Any())
                    {
                        CheckedNodes.Add(nodeToCheck);
                        WaitingNodes.AddRange(FindNeighboringNodes(nodeToCheck));
                    }
                }
            }
            else if (!cell) AddToListCheckedNodes(nodeToCheck);
        }
        return PathToTarget;
    }

    private void NullifyLists()
    {
        PathToTarget = new List<Vector2>();
        CheckedNodes = new List<Node>();
        WaitingNodes = new List<Node>();
    }

    private List<Node> FindNeighboringNodes(Node node)
    {
        var neighboringNodes = new List<Node>();
        neighboringNodes.Add(new Node(node.DistanceFromStartToNode + 1, new Vector2(node.CurrentPosition.x - 1, node.CurrentPosition.y), node.TargetPosition, node));
        neighboringNodes.Add(new Node(node.DistanceFromStartToNode + 1, new Vector2(node.CurrentPosition.x + 1, node.CurrentPosition.y), node.TargetPosition, node));
        neighboringNodes.Add(new Node(node.DistanceFromStartToNode + 1, new Vector2(node.CurrentPosition.x, node.CurrentPosition.y - 1), node.TargetPosition, node));
        neighboringNodes.Add(new Node(node.DistanceFromStartToNode + 1, new Vector2(node.CurrentPosition.x, node.CurrentPosition.y + 1), node.TargetPosition, node));
        return neighboringNodes;
    }

    private List<Vector2> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector2>();
        Node currentNode = node;
        while (currentNode.PreviousNode != null)
        {
            path.Add(new Vector2(currentNode.CurrentPosition.x, currentNode.CurrentPosition.y));
            currentNode = currentNode.PreviousNode;
        }
        return path;
    }

    private void AddToListCheckedNodes(Node node)
    {
        WaitingNodes.Remove(node);
        CheckedNodes.Add(node);
    }

    private void OnDrawGizmos()
    {
        foreach (var item in CheckedNodes)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector2(item.CurrentPosition.x, item.CurrentPosition.y), 0.1f);
        }
        if (PathToTarget != null)
        {
            foreach (var item in PathToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector2(item.x, item.y), 0.1f);
            }
        }
    }
}

public class Node
{
    public Node PreviousNode { get; set; }

    public Vector2 TargetPosition { get; set; }
    public Vector2 CurrentPosition { get; set; }

    public int Cost { get; set; } // F = G + H
    public int DistanceFromStartToNode { get; set; } // G
    public int DistanceFromNodeToTtarget { get; set; } // H

    public Node(int distanceFromStartToNode, Vector2 nodePosition, Vector2 targetPosition, Node previousNode)
    {
        CurrentPosition = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        DistanceFromStartToNode = distanceFromStartToNode;
        DistanceFromNodeToTtarget = (int)Mathf.Abs(TargetPosition.x - CurrentPosition.x) + (int)Mathf.Abs(TargetPosition.y - CurrentPosition.y);
        Cost = DistanceFromStartToNode + DistanceFromNodeToTtarget;
    }
}