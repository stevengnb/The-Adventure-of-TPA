using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager prm;
    private Grid grid;
    int t;

    public void Awake() {
        prm = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(findPath(startPos, targetPos));
    }

    public IEnumerator findPath(Vector3 startPos, Vector3 targetPos) {
        Stopwatch st = new Stopwatch();
        st.Start();
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if(targetNode.pass) {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closeSet = new HashSet<Node>();

            openSet.Add(startNode);

            while(openSet.Count > 0) {
                Node currNode = openSet.RemoveFirst();
                closeSet.Add(currNode);

                if(currNode == targetNode) {
                    st.Stop();
                    print("Path found: " + st.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node n in grid.GetNeighbours(currNode)) {
                    if(!n.pass || closeSet.Contains(n)) {
                        continue;
                    }

                    int movementCostToNeighbour = currNode.gCost + getDistance(currNode, n);
                    if(movementCostToNeighbour < n.gCost || !openSet.Contains(n)) {
                        n.gCost = movementCostToNeighbour;
                        n.hCost = getDistance(n, targetNode);
                        n.parent = currNode;

                        if(!openSet.Contains(n)) {
                            openSet.Add(n);
                        } else {
                            openSet.UpdateItem(n);
                        }
                    }
                }
            }
        }

        yield return null;
        if(pathSuccess) {
            waypoints = retracePath(startNode, targetNode);
        }
        prm.ProcessPathDone(waypoints, pathSuccess);
    }

    Vector3[] retracePath(Node start, Node end) {
        List<Node> path = new List<Node>();
        Node currNode = end;

        while(currNode != start) {
            path.Add(currNode);
            currNode = currNode.parent;
        }

        Vector3[] waypoints = simplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] simplifyPath(List<Node> path) {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if(directionNew != directionOld) {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }

    public int getDistance(Node a, Node b) {
        int distanceX = Mathf.Abs(a.gridX - b.gridX);
        int distanceY = Mathf.Abs(b.gridY - a.gridY);

        if(distanceX > distanceY) {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        } else {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }
}
