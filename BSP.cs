using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BSP : MonoBehaviour
{
    public static List<BoundsInt> Partitioning(BoundsInt splitSpace, int minWidth, int minHeight) {
        Queue<BoundsInt> rQueue = new Queue<BoundsInt>();
        List<BoundsInt> rList = new List<BoundsInt>();
        rQueue.Enqueue(splitSpace);

        while(rQueue.Count > 0) {
            var room = rQueue.Dequeue();
            if(room.size.z >= minHeight && room.size.x >= minWidth) {
                if(Random.value < 0.5f) {
                    if(room.size.z >= minHeight * 2) {
                        SplitHorizontally(minHeight, rQueue, room);
                    } else if(room.size.x >= minWidth * 2) {
                        SplitVertically(minWidth, rQueue, room);
                    } else {
                        rList.Add(room);
                    }
                } else {
                    if(room.size.x >= minWidth * 2) {
                        SplitVertically(minWidth, rQueue, room);
                    } else if(room.size.z >= minHeight * 2) {
                        SplitHorizontally(minHeight, rQueue, room);
                    } else {
                        rList.Add(room);
                    }
                }
            }
        }

        return rList;
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> rQueue, BoundsInt room) {
        var zSplit = Random.Range(1, room.size.z);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, room.size.y, zSplit));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y, room.min.z + zSplit), new Vector3Int(room.size.x, room.size.y, room.size.z - zSplit));
        rQueue.Enqueue(room1);
        rQueue.Enqueue(room2);
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> rQueue, BoundsInt room) {
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z), new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));
        rQueue.Enqueue(room1);
        rQueue.Enqueue(room2);
    }
}
