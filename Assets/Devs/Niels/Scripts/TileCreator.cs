using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreator : MonoBehaviour
{
    [SerializeField]
    GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                Instantiate(tile, new Vector3(i, 0, j), Quaternion.identity, this.transform);
            }
        }
    }

    // Update is called once per frame
    void Update() { }
}
