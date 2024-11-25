using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> Cosmetics = new List<GameObject>();

    [SerializeField]
    public List<Color> colors = new List<Color>();

    public List<ColorTracker> takenColors = new List<ColorTracker>();

    public List<GameObject> playerSelections = new List<GameObject>();
    [SerializeField]
    private GameObject playerSelect;
    private MenuPlayer menuPlayer;
    public List<GameObject> playerSelectors = new List<GameObject>();


    public List<GameObject> menuSelections = new List<GameObject>();



    private void Awake()
    {
        playerSelect = GameObject.Find("Player Select");
        for (int i = 1; i <= 4; i++)
        {
            GameObject playerMenu = GameObject.Find("Player " + i);
            playerSelections.Add(playerMenu);

        }

        colors = new List<Color>
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.black
        };
    }
}