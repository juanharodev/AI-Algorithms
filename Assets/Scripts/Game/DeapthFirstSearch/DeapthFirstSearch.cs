using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeapthFirstSearch : PathFindingAlgorithm
{
    public override string GetName() => "DFS";

    public override List<Node> Search(Node _start, Node _end)
    {
        Stack<Node> searching = new Stack<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Node currentNode = _start;
        _start.parent = _start;
        visited.Add(_start);

        //Search the graph
        while (currentNode != _end)
        {
            for (int i = 0; i < currentNode.children.Count; i++)
            {
                if (!visited.Contains(currentNode.children[i]))
                {
                    searching.Push(currentNode.children[i]);
                    searching.Peek().parent = currentNode;
                }
            }
            if (searching.Count == 0) { throw new System.Exception("There arer no nodes left for search\nPath not found"); }
            currentNode = searching.Pop();
            visited.Add(currentNode);
        }

        Debug.Log("Current node: " + currentNode.name);

        //Backtrack route
        List<Node> route = new List<Node>();
        while (currentNode != _start)
        {
            route.Add(currentNode);
            currentNode = currentNode.parent;
        }
        route.Add(_start);
        route.Reverse();

        return route;
    }
}
