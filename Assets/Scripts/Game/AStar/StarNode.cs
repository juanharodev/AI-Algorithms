using UnityEngine;
using System.Collections.Generic;

public class StarNode : MonoBehaviour
{
    public int cost;
    public List<StarNode> siblings = new List<StarNode>();
    public StarNode parent = null;
    float yOffset = 0.5f;

    public bool IsBlocked => cost == int.MaxValue;

    public void SetCost(int _cost)
    {
        cost = _cost == int.MaxValue? int.MaxValue : _cost;
        GetComponent<Renderer>().material.SetFloat("_Cost", _cost);
    }

    #region Visuals
    private void OnMouseEnter()
    {
        Vector3 upPos = transform.position;
        upPos.y += yOffset;
        transform.position = upPos;
    }

    private void OnMouseExit()
    {
        Vector3 downPos = transform.position;
        downPos.y -= yOffset;
        transform.position = downPos;
    }
    #endregion
}