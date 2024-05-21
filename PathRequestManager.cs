using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currPathReq;
    Pathfinding pf;
    bool isProcessPath;

    static PathRequestManager instance;

    void Awake() {
        instance = this;
        pf = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) {
        PathRequest newReq = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newReq);
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if(!isProcessPath && pathRequestQueue.Count > 0) {
            currPathReq = pathRequestQueue.Dequeue();
            isProcessPath = true;
            pf.StartFindPath(currPathReq.pathStart, currPathReq.pathEnd);
        }
    }

    public void ProcessPathDone(Vector3[] path, bool success) {
        currPathReq.callback(path, success);
        isProcessPath = false;
        TryProcessNext();
    }

    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callbackk) {
            pathStart = start;
            pathEnd = end;
            callback = callbackk;
        }
    }
}
