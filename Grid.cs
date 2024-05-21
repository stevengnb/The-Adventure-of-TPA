using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public LayerMask unpassableMask;
    public Vector2 gridSize;
    public float nodeRadius;
    Node[,] grid;
    float diameter;
    int gridSizeX, gridSizeY;

    public void Start() {
        diameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x/diameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y/diameter);
        createGrid();
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }

    public void createGrid() {
        grid =  new Node[gridSizeX, gridSizeY];
        Vector3 worldBotomLeft = transform.position - Vector3.right * gridSize.x/2 - Vector3.forward * gridSize.y/2;
 
        for(int i = 0; i < gridSizeX; i++) {
            for(int j = 0; j < gridSizeY; j++) {
                Vector3 worldPoint = worldBotomLeft + Vector3.right * (i * diameter + nodeRadius) + Vector3.forward * (j * diameter + nodeRadius);
                bool pass = !(Physics.CheckSphere(worldPoint, nodeRadius, unpassableMask));

                grid[i, j] = new Node(pass, worldPoint, i, j);
            }
        }
    }

    public List<Node> GetNeighbours(Node node) {
        List<Node> neighbours = new List<Node>();

        for(int i = -1; i < 2; i++) {
            for(int j = -1; j < 2; j++) {
                if(i == 0 && j == 0) {
                    continue;
                }
                int checkX = node.gridX + i;
                int checkY = node.gridY + j;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        Vector3 localPosition = worldPosition - transform.position;
        float percentX = Mathf.Clamp01(localPosition.x / gridSize.x + 0.5f);
        float percentY = Mathf.Clamp01(localPosition.z / gridSize.y + 0.5f);

        int x = Mathf.FloorToInt(Mathf.Clamp01(percentX) * (gridSizeX - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp01(percentY) * (gridSizeY - 1));
        return grid[x, y];
    }

    public void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 1, gridSize.y));

        if(grid != null && displayGridGizmos) {
            foreach (Node n in grid) {
                if(n.pass) {
                    Gizmos.color = Color.white;
                } else {
                    Gizmos.color = Color.red;
                }
                
                Gizmos.DrawCube(n.worldPos, Vector3.one * (diameter - 0.1f));
            }
        }
    }
}
