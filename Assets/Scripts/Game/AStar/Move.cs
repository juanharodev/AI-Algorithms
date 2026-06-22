using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    bool isMoving = false;
    public bool IsMoving => isMoving;
    public float minDistance = 0.1f;
    public float speed = 20f;


    public void MoveInPath(List<Transform> _path)
    {
        if (isMoving) {return;}
        isMoving = true;
        StartCoroutine(Moving(_path));
    }

    IEnumerator Moving(List<Transform> _path)
    {
        int i = 0;
        while (i < _path.Count)
        {
            transform.rotation = Quaternion.FromToRotation(transform.position, _path[i].position);  
            while (minDistance < Vector3.Distance(transform.position, _path[i].position))
            {
                transform.Translate((transform.position - _path[i].transform.position) * (Time.deltaTime * speed));
                yield return null;
            }

            i++;
        }
        isMoving = false;
    }
}
