using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    public List<Sprite> Cosmetics = new List<Sprite>();

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
    [SerializeField]
    public GameObject playersParent;
    [SerializeField]
    private List<GameObject> playerSpawns = new List<GameObject>();
    public List<GameObject> slimes = new List<GameObject>();



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

        // Initialize menuPlayers
        GetPlayers();
    }

    public void GetPlayers()
    {
        menuPlayers = GameObject.FindObjectsOfType<MenuPlayer>();

    }

    private void Update()
    {
        if (menuPlayers != null && menuPlayers.Length > 1)
        {
            bool canStart = true;

            for (int i = 0; i < menuPlayers.Length; i++)
            {
                if (menuPlayers[i] != null && menuPlayers[i].isReady == false)
                {
                    canStart = false;
                }
            }

            if (canStart == true)
            {
                canStart = false;
                foreach (GameObject players in playerSelectors)
                {
                    // Set the parent of the player selector to playersParent
                    players.GetComponent<MenuPlayer>().enabled = false;
                    players.GetComponent<PlayerInput>().enabled = false;
                    players.GetComponent<BoxCollider2D>().enabled = false;


                }
                for (int i = slimes.Count - 1; i >= 0; i--)
                {

                    slimes[i].transform.position = playerSpawns[i].transform.position;
                }
                GameObject.Find("Canvas").SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

}