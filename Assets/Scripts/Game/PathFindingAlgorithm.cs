using UnityEngine;
using System.Collections.Generic;

public abstract class PathFindingAlgorithm
{
    public abstract List<Node> Search(Node _start, Node _end);
    public abstract string GetName();
}
