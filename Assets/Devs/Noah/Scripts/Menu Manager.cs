using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Manages the player selection menu and character customization
public class MenuManager : MonoBehaviour
{
    // List of available cosmetic sprites for player customization
    [SerializeField]
    public List<Sprite> Cosmetics = new List<Sprite>();

    // List of available colors for player selection
    [SerializeField]
    public List<Color> colors = new List<Color>();

    // Materials used for player color customization
    [SerializeField]
    public List<Material> playerColors = new List<Material>();

    // Tracks which colors are currently in use by players
    public List<ColorTracker> takenColors = new List<ColorTracker>();

    // References to player selection UI elements
    public List<GameObject> playerSelections = new List<GameObject>();
    [SerializeField]
    private GameObject playerSelect;
    private MenuPlayer menuPlayer;
    public List<GameObject> playerSelectors = new List<GameObject>();

    // References to menu UI elements and player objects
    public List<GameObject> menuSelections = new List<GameObject>();
    public MenuPlayer[] menuPlayers;
    [SerializeField]
    public GameObject playersParent;
    [SerializeField]
    private List<GameObject> playerSpawns = new List<GameObject>();
    public List<GameObject> slimes = new List<GameObject>();

    public bool canStart = false;
    private GameManager gameManager;

    // Initializes menu components and color options
    private void Awake()
    {
        // Find the main player selection screen
        playerSelect = GameObject.Find("Player Select");

        // Find and store references to all 4 player menu objects
        for (int i = 1; i <= 4; i++)
        {
            GameObject playerMenu = GameObject.Find("Player " + i);
            playerSelections.Add(playerMenu);
        }

        // Initialize the color palette with predefined RGB values
        colors = new List<Color>
        {
            // Each color is normalized by dividing by 255 to get proper RGB values
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

        // Initialize player references
        GetPlayers();

        gameManager = FindObjectOfType<GameManager>();
    }

    // Finds and stores references to all active MenuPlayer components
    public void GetPlayers()
    {
        // Find all active MenuPlayer components in the scene
        menuPlayers = GameObject.FindObjectsOfType<MenuPlayer>();
    }

    // Checks if all players are ready and starts the game when conditions are met
    private void Update()
    {
        // Only proceed if there are at least 2 players
        if (menuPlayers != null && menuPlayers.Length > 0)
        {
            canStart = true;

            // Check if all players have indicated they are ready
            for (int i = 0; i < menuPlayers.Length; i++)
            {
                if (menuPlayers[i] != null && menuPlayers[i].isReady == false)
                {
                    canStart = false;
                }
            }

            // If all players are ready, start the game
            if (canStart == true)
            {
                gameManager.canStart = true;

                canStart = false;
                // Disable player selection controls
                foreach (GameObject players in playerSelectors)
                {
                    // Disable player selection components
                    players.GetComponent<MenuPlayer>().enabled = false;
                    players.GetComponent<PlayerInput>().enabled = false;
                    players.GetComponent<BoxCollider2D>().enabled = false;
                }

                // Move slimes to their starting positions
                for (int i = slimes.Count - 1; i >= 0; i--)
                {
                    slimes[i].transform.position = playerSpawns[i].transform.position;
                }

                // Hide the player selection screen
                GameObject.Find("Player Select").SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

}