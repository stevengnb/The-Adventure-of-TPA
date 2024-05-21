    using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridPrim : MonoBehaviour
{
    public Vector2 gridSize;
    public float nodeRadius;
    public NodePrim[,] grid;
    public float diameter;
    int gridSizeX, gridSizeY;
    float gapNode = 3f;
    public int markInterval = 10;
    private int stepCounter = 0;

    public void Awake() {
        diameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x/(diameter + gapNode));
        gridSizeY = Mathf.RoundToInt(gridSize.y/(diameter + gapNode));
        createGrid();
        Debug.Log(grid);
        GeneratePath();
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    public void createGrid() {
        grid =  new NodePrim[gridSizeX, gridSizeY];
        Vector3 worldBotomLeft = transform.position - Vector3.right * gridSize.x/2 - Vector3.forward * gridSize.y/2;
 
        for(int i = 0; i < gridSizeX; i++) {
            for(int j = 0; j < gridSizeY; j++) {
                Vector3 worldPoint = worldBotomLeft + Vector3.right * (i * (diameter + gapNode) + nodeRadius) + Vector3.forward * (j * (diameter + gapNode) + nodeRadius);

                grid[i, j] = new NodePrim(worldPoint, i, j);
            }
        }
    }

    public void GeneratePath() {
        Stack<NodePrim> frontier = new Stack<NodePrim>();
        NodePrim start = grid[0, 0];
        frontier.Push(start);

        while(frontier.Count > 0) {
            NodePrim currNode = frontier.Peek();
            frontier.Pop();

            currNode.isVisited = true;

            stepCounter++;

            if (stepCounter == markInterval) {
                currNode.isMark = true;
                stepCounter = 0;
            }

            if(currNode.parent != null) {
                currNode.parent.children.Add(currNode);
            }

            foreach (NodePrim np in GetNeighbours(currNode)) {
                np.parent = currNode;
                frontier.Push(np);
            }
        }
    }

    public List<NodePrim> GetNeighbours(NodePrim np) {
        List<NodePrim> neighbours = new List<NodePrim>();

        for (int i = -1; i < 2; i++) {
            for (int j = -1; j < 2; j++) {
                if ((i == 0 && j == 0) || (Mathf.Abs(i) + Mathf.Abs(j) == 2)) {
                    continue;
                }

                int checkX = np.gridX + i;
                int checkY = np.gridY + j;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    if(!(grid[checkX, checkY].isVisited)) {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }
        }

        return ShuffleList(neighbours);
    }

    private List<NodePrim> ShuffleList(List<NodePrim> list) {
    int count = list.Count;
    System.Random random = new System.Random();

        while (count > 1) {
            count--;
            int randomIndex = random.Next(count + 1);
            NodePrim temp = list[randomIndex];
            list[randomIndex] = list[count];
            list[count] = temp;
        }

    return list;
    }


    public NodePrim NodeFromWorldPoint(Vector3 worldPosition) {
        Vector3 localPosition = worldPosition - transform.position;
        float percentX = Mathf.Clamp01(localPosition.x / gridSize.x + 0.5f);
        float percentY = Mathf.Clamp01(localPosition.z / gridSize.y + 0.5f);

        int x = Mathf.FloorToInt(Mathf.Clamp01(percentX) * (gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp01(percentY) * (gridSizeY - 1));
        return grid[x, y];
    }
}
