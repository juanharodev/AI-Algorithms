using UnityEngine;
using System.Collections.Generic;

public class CSNode
{
    public HashSet<Edge> edges;

}

public struct Edge
{
    CSNode A;
    CSNode B;
    int weight;
}