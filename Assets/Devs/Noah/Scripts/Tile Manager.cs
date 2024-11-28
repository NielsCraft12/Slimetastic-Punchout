using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tiles = new List<GameObject>();

    private void Awake()
    {
        foreach(Transform _child in gameObject.transform)
        {
            tiles.Add(_child.gameObject);
        }
    }
}