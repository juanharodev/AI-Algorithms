using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine; 
using UnityEngine.InputSystem;

public class AStarPathFinding : MonoBehaviour
{
    public static AStarPathFinding Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    [SerializeField] LayerMask nodeMask;
    [SerializeField] float distance;

    #region InputActions
    PathFinderControls controls;
    private void Start()
    {
        //Set up controller
        controls = new PathFinderControls();
        controls.Level.Enable();
        controls.Level.Start.performed += SetStart;
        controls.Level.End.performed += SetEnd;


    }
    private void OnDestroy()
    {
        //Clean up controller
        controls.Level.Disable();
        controls.Level.Start.performed -= SetStart;
        controls.Level.End.performed -= SetEnd;
    }
    private void SetStart(InputAction.CallbackContext context)
    {
        FindNode(true);
    }
    private void SetEnd(InputAction.CallbackContext context)
    {
        FindNode(false);
    }
    #endregion

    #region NodeFinding
    void FindNode(bool isStart)
    {
        Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * distance, Color.magenta, 1.0f);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, distance, nodeMask))
        {
            if (hitInfo.collider.gameObject.TryGetComponent<StarNode>(out StarNode _node))
            {
                if (isStart)
                {
                    SetStart(_node);
                }
                else
                {
                    SetEnd(_node);
                }
            }
            else
            {
                Debug.Log("No node was selected");
            }
        }
    }
    
    StarNode startNode;
    void SetStart(StarNode _startNode)
    {
        if (startNode) { MakeSmall(startNode.gameObject); }
        if (_startNode == endNode) { endNode = null; }
        startNode = _startNode;
        MakeBig(startNode.gameObject);
        Debug.Log("Star set" +  startNode.name);
    }

    StarNode endNode;
    public Transform GetEndNodeTransform => endNode.transform;
    private void SetEnd(StarNode _endNode)
    {
        if (endNode) { MakeSmall(endNode.gameObject); }
        if(endNode == startNode) { startNode = null; }
        endNode = _endNode;
        MakeBig(endNode.gameObject);
        Debug.Log("End set" + endNode.name);
    }
    #endregion

    #region PathFinding
    List<StarNode> path;
    public void Search()
    {
        //Node validation
        if (startNode == null || endNode == null)
        {
            Debug.Log("Path cant be established\nPlease select a start and end node");
            return;
        }
        if(startNode.IsBlocked)
        {
            Debug.Log("Start node is blocked\nPlease choose a valid node");
            return ;
        }
        if (endNode.IsBlocked)
        {
            Debug.Log("End node is blocked\nPlease choose a valid node");
            return ;
        }

        if(path != null)
        {
            foreach (StarNode node in path)
            {
                node.gameObject.GetComponent<Renderer>().material.SetInt("_IsShowingPath", 0);
                MakeSmall(node.gameObject);
            }
        }
        

        Dictionary<StarNode,float> openList = new Dictionary<StarNode,float>();
        HashSet<StarNode> closeList = new HashSet<StarNode>();

        StarNode currentNode = startNode;
        openList.Add(currentNode, currentNode.cost);

        while (openList.Count > 0 && currentNode != endNode) 
        {
            currentNode = GetLowest(openList);
            float currentCost = openList[currentNode];
            openList.Remove(currentNode);
            closeList.Add(currentNode);

            //Add valid sibling
            foreach(StarNode _sibling in currentNode.siblings)
            {
                if(_sibling.IsBlocked){ continue; }
                //Sibling has already been process
                if (closeList.Contains(_sibling)){ continue; }

                float gcost = currentCost + _sibling.cost;
                float hcost = Vector3.Distance(_sibling.transform.position,endNode.transform.position);
                float fCost = gcost + hcost;

                //Sibling is not registered yet 
                if (!openList.ContainsKey(_sibling)) 
                { 
                    openList.Add(_sibling, fCost); 
                    _sibling.parent = currentNode;
                }
                //Siblig is registered
                else 
                {
                    //New path is shorter than the previous one
                    if (fCost < openList[_sibling])
                    {
                        _sibling.parent = currentNode;
                        openList[_sibling] = fCost;
                    }
                }
            }
        }

        //Path found
        if(currentNode == endNode)
        {
            path = new List<StarNode>();
            path.Add(currentNode);
            while(currentNode != startNode)
            {
                path.Add(currentNode.parent);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            StartCoroutine(DrawPathRoutine(path));
        }
        //Path not found
        else
        {
            Debug.Log("A path could not be found");
        }

        //Reset node parents
        foreach(StarNode node in closeList)
        {
            node.parent = null;
        }
        foreach(KeyValuePair<StarNode,float> node in openList)
        {
            node.Key.parent = null;
        }
    }

    IEnumerator DrawPathRoutine(List<StarNode> path)
    {
        foreach(StarNode node in path)
        {
            node.gameObject.GetComponent<Renderer>().material.SetInt("_IsShowingPath", 1);
            MakeBig(node.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void MakeBig(GameObject _gameObject)
    {
        _gameObject.transform.localScale = new Vector3(1.25f,1.25f,1.25f);
    }
    void MakeSmall(GameObject _gameObject)
    {
        _gameObject.transform.localScale = Vector3.one;
    }

    StarNode GetLowest(Dictionary<StarNode,float> _nodes)
    {
        KeyValuePair<StarNode,float> lowest = new KeyValuePair<StarNode, float>(startNode,float.MaxValue);
        foreach (KeyValuePair<StarNode,float> _node in  _nodes)
        {
            if(_node.Value < lowest.Value)
            {
                lowest = _node;
            }
        }
        return lowest.Key;
    }
    #endregion
}
