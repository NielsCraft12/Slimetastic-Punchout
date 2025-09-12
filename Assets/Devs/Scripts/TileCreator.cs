using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCreator : MonoBehaviour
{
    [SerializeField]
    GameObject tile;

    [SerializeField]
    private int mapSizeX = 3;
    [SerializeField]
    private int mapSizeY = 2;

    [SerializeField]
    float spaceX;
    [SerializeField]
    float spaceZ;

    private Transform groundTileChild;

    void Start()
    {
        groundTileChild = GetComponentInChildren<Transform>();

        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                Instantiate(tile, new Vector3(groundTileChild.position.x + i * spaceX, 0, groundTileChild.position.z + j * spaceZ), Quaternion.identity, this.transform);
            }
        }
    }
}
