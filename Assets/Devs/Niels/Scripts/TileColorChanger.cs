using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class TileColorChanger : MonoBehaviour
{
    [SerializeField]
    Material materialColor;

    private enum PlayerColors
    {
        Red,
        Blue,
        Green,
        Yellow,
        Black
    }

    [SerializeField] PlayerColors playerColor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            foreach(Transform child in other.gameObject.transform)
            {
                if (child.gameObject.name == "Tile Color")
                {
                    Renderer renderer = child.gameObject.GetComponent<Renderer>();
                    renderer.material = materialColor;
                }
            }
        }
    }
}
