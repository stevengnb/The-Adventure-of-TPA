using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllObstacle : MonoBehaviour
{
    [Header("Binary Space Partitions")]
    public bool isDisplayBSP = false;
    public bool isDisplayPrim = true;
    [SerializeField] private Terrain terrain;
    [SerializeField] private int minWidth;
    [SerializeField] private int minHeight;
    [SerializeField] private int sizeXField;
    [SerializeField] private int sizeZField;
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject parentObstacle;
    private GridPrim gridPrim;
    private NodePrim[,] grid;
    private List<BoundsInt> rooms = new List<BoundsInt>();

    private void Awake() {
        BoundsInt baseBoundsInt = new BoundsInt(0, 0, 0, sizeXField, 0, sizeZField);
        rooms = BSP.Partitioning(baseBoundsInt, minWidth, minHeight);
        gridPrim = GetComponent<GridPrim>();
        grid = gridPrim.grid;
        GenerateObstacle();
    }

    private void GenerateObstacle() {
        if(rooms != null) {
            foreach(BoundsInt room in rooms) {
                if(grid != null) {
                    foreach(NodePrim np in grid) {
                        if(np.isMark) {
                            if((np.worldPos.x >= (room.min.x + (terrain.terrainData.size.x / 6f)) && np.worldPos.x <= (room.max.x + (terrain.terrainData.size.x / 6f))) && (np.worldPos.z >= (room.min.z + (terrain.terrainData.size.z / 6f)) && np.worldPos.z <= (room.max.z + (terrain.terrainData.size.z / 6f)))) {
                                Vector3 obsPos = new Vector3(np.worldPos.x, 3.11f, np.worldPos.z);

                                bool isNotSpawn = Physics.CheckSphere(obsPos, 0.75f, LayerMask.GetMask("NoSpawn"));
                                bool isNotObstacle = Physics.CheckSphere(obsPos, 0.5f, LayerMask.GetMask("Obstacle"));

                                if(!isNotSpawn && !isNotObstacle) {
                                    Instantiate(obstacle, obsPos, transform.rotation, parentObstacle.transform);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos() {
        if (rooms != null) {
            if(isDisplayBSP) {
                foreach (var room in rooms) {
                    Gizmos.color = Color.gray;

                    Vector3 roomCenterOffset = new Vector3(room.center.x, 10f, room.center.z) + new Vector3(terrain.terrainData.size.x / 6f, 0, terrain.terrainData.size.z / 6f);
                    Gizmos.DrawCube(roomCenterOffset, new Vector3(room.size.x, 2, room.size.z));
                }
            }
        }

        if(grid != null) {
            if(isDisplayPrim) {
                foreach(NodePrim np in grid) {
                    if(np.isVisited) {
                        Gizmos.color = Color.cyan;
                    } else {
                        Gizmos.color = Color.white;
                    }

                    if(np.isMark){
                        Gizmos.color = Color.magenta;
                    }

                    Gizmos.DrawCube(np.worldPos, Vector3.one * (gridPrim.diameter - 0.1f));

                    if (np.parent != null) {
                        Gizmos.color = Color.black;
                        Gizmos.DrawLine(np.worldPos, np.parent.worldPos);
                    }
                }
            }
        }
    }
}
