using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : PathFindingAlgorithm
{
    public override string GetName() => "BFS";

    public override List<Node> Search(Node _start, Node _end)
    {
        Queue<Node> searching = new Queue<Node>();
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
                    visited.Add(currentNode.children[i]);
                    searching.Enqueue(currentNode.children[i]);
                    currentNode.children[i].parent = currentNode;
                }
            }
            if (searching.Count == 0) { throw new System.Exception("There are no nodes left for search\nPath not found"); }
            currentNode = searching.Dequeue();
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
