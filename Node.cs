using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool pass;
    public Vector3 worldPos;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndexx;

    public Node(bool passs, Vector3 worldPostition, int x, int y) {
        pass = passs;
        worldPos = worldPostition;
        gridX = x;
        gridY = y;
    }

    public int fCost {
        get { return gCost + hCost; }
    }

    public int heapIndex {
        get {
            return heapIndexx;
        }
        set {
            heapIndexx = value;
        }
    }

    public int CompareTo(Node node) {
        int compare = fCost.CompareTo(node.fCost);

        if(compare == 0) {
            compare = hCost.CompareTo(node.hCost);
        }

        return -compare;
    }
}
