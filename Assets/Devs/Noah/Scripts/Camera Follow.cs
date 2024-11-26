using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class CameraFollow : MonoBehaviour
{
    private GameManager gameManager;

    private GameObject[] players;

    [SerializeField] private Vector3 offset;
    private Vector3 velocity;
    [SerializeField] private float smoothTime = .5f;

    //Zooming
    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomLimiter = 50f;

    //Camera
    private Camera cam;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        players = gameManager.playerArray;

        if (players.Length == 0)
        { 
            return;
        }

        MoveCam();
        ZoomCam();
    }

    private void MoveCam()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void ZoomCam()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        var _bounds = new Bounds(players[0].transform.position, Vector3.zero);

        for (int i = 0; i < players.Length; i++)
        {
            _bounds.Encapsulate(players[i].transform.position);
        }

        return _bounds.size.x;
    }

    //This function is based of the brackeys tutorial: "MULTIPLE TARGET CAMERA in Unity"
    private Vector3 GetCenterPoint()
    {
        if (players.Length == 1)
        {
            return players[0].transform.position;
        }

        var _bounds = new Bounds(players[0].transform.position, Vector3.zero);

        for (int i = 0; i < players.Length; i++) 
        {
            _bounds.Encapsulate(players[i].transform.position);
        }

        return _bounds.center;
    }
}
