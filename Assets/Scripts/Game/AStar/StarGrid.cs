using System.Collections.Generic;
using UnityEngine;

public class StarGrid : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeField] GameObject node;

    [SerializeField] LayerMask nodeMask;

    List<List<StarNode>> gridMatrix;

    private void Awake()
    {
        
    }

    public void GenerateGrid()
    {
        node.SetActive(true);
        if (gridMatrix != null)
        {
            for (int i = 0; i < gridMatrix.Count; i++)
            {
                for(int j = 0; j < gridMatrix[i].Count; j++)
                {
                    Destroy(gridMatrix[i][j].gameObject);
                }
            }
        }
        gridMatrix = Grid();
        node.SetActive(false);
    }

    List<List<StarNode>> Grid()
    {
        float xOffset = node.transform.localScale.x + 0.25f;
        float zOffset = node.transform.localScale.z + 0.25f;

        gridMatrix = new List<List<StarNode>>();

        for (int i = 0; i < width; i++)
        {
            gridMatrix.Add(new List<StarNode>());
            for (int j = 0; j < height; j++)
            {
                int cost = Random.Range(0, 10);
                if (cost > 7) { cost  = int.MaxValue; }
                float xPos = xOffset * j;
                float zPos = zOffset * i;
                Vector3 postion = new Vector3(xPos, 0, zPos);
                GameObject cube = Instantiate(node, postion, node.transform.rotation);
                cube.name = "[" + i + "," + j + "]";
                StarNode currentNode = cube.AddComponent<StarNode>();
                currentNode.SetCost(cost);
                gridMatrix[i].Add(currentNode);
            }
        }
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //a
                if (CheckInRange(i - 1, j - 1, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i - 1][j - 1]);
                }

                if (CheckInRange(i - 1, j, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i - 1][j]);
                }
                if (CheckInRange(i - 1, j + 1, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i - 1][j + 1]);
                }

                //b
                if (CheckInRange(i, j - 1, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i][j - 1]);
                }
                if (CheckInRange(i, j + 1, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i][j + 1]);
                }

                //c
                if (CheckInRange(i + 1, j - 1, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i + 1][j - 1]);
                }
                if (CheckInRange(i + 1, j, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i + 1][j]);
                }
                if (CheckInRange(i + 1, j + 1, 0, width, 0, height))
                {
                    gridMatrix[i][j].siblings.Add(gridMatrix[i + 1][j + 1]);
                }
            }
        }
        return gridMatrix;
    }

    bool CheckInRange(float x, float y, float minX, float maxX, float minY, float maxY)
    {
        return (x >= minX) && (x < maxX) && (y >= minY) && (y < maxY);
    }
}
