using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class Node : MonoBehaviour
{
    public List<Node> children;
    public Node parent;
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        try
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        catch
        {
            Debug.Log("Node does not contain a mesh");
        }
    }
}
