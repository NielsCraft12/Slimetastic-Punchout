using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class TileColorChanger : MonoBehaviour
{
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject playerEyes;

    [SerializeField] int colorSelected = 0;

    public List<Color> colors = new List<Color>();

    public List<Material> playerColors = new List<Material>();
    private void Awake()
    {
        playerBody = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).gameObject;
        playerEyes = transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).gameObject;

        colors = new List<Color>
        {
        //Red
         new Color(233 / 255.0f, 18 / 255.0f, 18 / 255.0f, 1),
         //Blue
         new Color(100 / 255.0f, 74 / 255.0f, 255 / 255.0f, 1),
         // Brown
         new Color(187 / 255.0f, 113 / 255.0f, 81 / 255.0f, 1),
         // Cyan
         new Color(14 / 255.0f, 200 / 255.0f, 199 / 255.0f, 1),
         // Gray
         new Color(113 / 255.0f, 113 / 255.0f, 113 / 255.0f, 1),
         // Green
         new Color(76 / 255.0f, 207 / 255.0f, 16 / 255.0f, 1),
         // Orange
         new Color(230 / 255.0f, 94 / 255.0f, 20 / 255.0f, 1),
         // Pink
         new Color(218 / 255.0f, 13 / 255.0f, 149 / 255.0f, 1),
         // Purple
         new Color(142 / 255.0f, 17 / 255.0f, 217 / 255.0f, 1),
         // Yellow
         new Color(231 / 255.0f, 216 / 255.0f, 20 / 255.0f, 1)
        };
    }

    private void Update()
    {
        if (playerBody == null || playerEyes == null)
        {
            playerBody = transform.GetChild(1).transform.GetChild(0).transform.GetChild(1).gameObject;
            playerEyes = transform.GetChild(1).transform.GetChild(1).transform.GetChild(1).gameObject;
        }
        playerBody.GetComponent<Renderer>().material = playerColors[colorSelected];
        playerEyes.GetComponent<Renderer>().material = playerColors[colorSelected];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            foreach (Transform child in other.gameObject.transform)
            {
                if (child.gameObject.name == "Tile Color")
                {
                    Renderer renderer = child.gameObject.GetComponent<Renderer>();
                    renderer.material.color = colors[colorSelected];
                }
            }
        }
    }
}
