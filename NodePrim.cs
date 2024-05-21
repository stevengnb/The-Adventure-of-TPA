using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePrim 
{
    public int gridX;
    public int gridY;
    public Vector3 worldPos;
    public bool isVisited = false;
    public bool isMark = false;
    public List<NodePrim> children = new List<NodePrim>();
    public NodePrim parent;

    public NodePrim(Vector3 worldPos, int x, int y) {
        this.worldPos = worldPos;
        gridX = x;
        gridY = y;
    }

    public void newChild(NodePrim np) {
        children.Add(np);
    }
}   
