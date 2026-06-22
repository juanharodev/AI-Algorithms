using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class PathFinding : MonoBehaviour
{
    #region Singleton
    public static PathFinding Instance;

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
    #endregion

    #region Algorithms
    List<PathFindingAlgorithm> algorithms;
    int currentAlgorithm;
   
    DeapthFirstSearch deapthFirstSearch;
    BreadthFirstSearch breadthFirstSearch;

    #endregion

    public Node start;
    public Node end;

    public GameObject startModel;
    public GameObject endModel;

    public List<Node> nodes;

    public TextMeshProUGUI algotrithmNameDisplay;

    public LineRenderer path;
    public LayerMask nodeMask;
    public float distance = 10000;

    PathFinderControls controls;

    public void SetStart(Node _start)
    {
        if(_start == start) { return; }

        if(_start == end) { 
            
            end = null;
            endModel.SetActive(false);
        }

        if (start != null) 
        {
            start.meshRenderer.enabled = true;
        }
        start = _start;
        start.meshRenderer.enabled = false;

        startModel.transform.position = start.transform.position;
        startModel.SetActive(true);
        
    }
    public void SetEnd(Node _end)
    {
        if (_end == end) { return; }

        if (_end == start) 
        {
            start = null;
            startModel.SetActive(false);
        }

        if (end != null)
        {
            end.meshRenderer.enabled = true;
        }
        end = _end;
        end.meshRenderer.enabled = false;

        endModel.transform.position = end.transform.position;
        endModel.SetActive(true);

    }
    
    void FindSphere(bool selectStart)
    {
        Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * distance, Color.magenta, 1.0f);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hitInfo, distance, nodeMask)){
            if (hitInfo.collider.gameObject.TryGetComponent<Node>(out Node _node))
            {
                if (selectStart)
                {
                    SetStart(_node);
                }
                else
                {
                    SetEnd(_node);
                }
            }
        }
    }

    public void ChangeMethod()
    {
        currentAlgorithm++;
        currentAlgorithm %= algorithms.Count;
        algotrithmNameDisplay.text = algorithms[currentAlgorithm].GetName();
    }

    public void Search()
    {
        //Check if te search is ready to start
        if(!start || !end)
        {
            Debug.Log("Rute not selected");
            return;
        }
        //Clean nodes
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].parent = null;
        }

        //Search route
        List<Node> route = algorithms[currentAlgorithm].Search(start,end);

        //Print route
        Debug.Log("Start: " + route[0].name);
        path.SetPosition(0, route[0].transform.position);
        for (int i = 1; i < route.Count-1; i++)
        {
            Debug.Log(route[i].name);
            path.SetPosition(i, route[i].transform.position);
        }
        path.SetPosition(route.Count-1, route[^1].transform.position);
        Debug.Log("End: " + route[^1]);
        //Fix extra points in line renderer xd, god i hate line renderer
        for(int i = route.Count; i < path.positionCount; i++)
        {
            path.SetPosition(i, route[^1].transform.position);
        }


        //Reset map values
        startModel.SetActive(false);
        endModel.SetActive(false);
        end.meshRenderer.enabled = true;
        start.meshRenderer.enabled = true;
        start = null;
        end = null;
    }

    private void Start()
    {
        //Turn off models
        startModel.SetActive(false);
        endModel.SetActive(false);

        //Initialize algorithms
        algorithms = new List<PathFindingAlgorithm>();
        deapthFirstSearch = new DeapthFirstSearch();
        breadthFirstSearch = new BreadthFirstSearch();
        
        currentAlgorithm = 0;

        algorithms.Add(deapthFirstSearch);
        algorithms.Add(breadthFirstSearch);

        ChangeMethod();

        //Set controls
        controls = new PathFinderControls();
        controls.Level.Enable();
        controls.Level.Start.performed += SetStartNode;
        controls.Level.End.performed += SetEndNode;
    }

    //Register input actions
    private void SetEndNode(InputAction.CallbackContext context)
    {
        FindSphere(false);
    }

    private void SetStartNode(InputAction.CallbackContext context)
    {
        FindSphere(true);
    }

    //Eliminate input actions
    public void OnDestroy()
    {
        controls.Level.Start.performed -= SetStartNode;
        controls.Level.End.performed -= SetEndNode;
        controls.Level.Disable();
    }
}
