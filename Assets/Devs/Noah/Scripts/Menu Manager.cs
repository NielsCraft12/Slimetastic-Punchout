using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    public List<GameObject> Cosmetics = new List<GameObject>();

    [SerializeField]
    public List<Color> colors = new List<Color>();

    [SerializeField]
    public List<Material> playerColors = new List<Material>();

    public List<ColorTracker> takenColors = new List<ColorTracker>();

    public List<GameObject> playerSelections = new List<GameObject>();
    [SerializeField]
    private GameObject playerSelect;
    private MenuPlayer menuPlayer;
    public List<GameObject> playerSelectors = new List<GameObject>();


    public List<GameObject> menuSelections = new List<GameObject>();
    public MenuPlayer[] menuPlayers;





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

    public void GetPlayers()
    {
        menuPlayers = GameObject.FindObjectsOfType<MenuPlayer>();

    }

    private void Update()
    {
        if (menuPlayers.Length > 1)
        {
            bool canStart = true;

            for (int i = 0; i < menuPlayers.Length; i++)
            {
                if (menuPlayers[i].isReady == false)
                {
                    canStart = false;
                }
            }

            if (canStart == true)
            {
                Debug.Log("iedereen ready");
            }
        }
    }

}