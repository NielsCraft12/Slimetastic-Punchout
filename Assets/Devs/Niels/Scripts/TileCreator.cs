using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreator : MonoBehaviour
{
    [SerializeField]
    GameObject tile;

    [SerializeField]
    private int mapSize = 25;

    private Transform groundTileChild;

    void Start()
    {
        groundTileChild = GetComponentInChildren<Transform>();

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                Instantiate(tile, new Vector3(groundTileChild.position.x + i * 6, 0, groundTileChild.position.z + j * 6), Quaternion.identity, this.transform);
            }
        }
    }
}
