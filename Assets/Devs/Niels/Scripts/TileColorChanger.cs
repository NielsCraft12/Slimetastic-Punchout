using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileColorChanger : MonoBehaviour
{
    [SerializeField]
    Material materialColor;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            renderer.material = materialColor;
        }
    }
}
