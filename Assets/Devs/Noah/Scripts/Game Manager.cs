using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Scene currentScene;
    private string currentSceneName;

    private GameObject canvas;

    public GameObject[] playerArray;

    public int[] playerScore;

    private PlayerInputManager playerInputManager;

    [SerializeField] private float gameTimer = 120f;

    private TextMeshProUGUI timerText;

    public bool canStart = false;
    public bool isPerformingWin = false;
    private bool canCalculate = true;
    private bool initializedPlayerScoreSize = false;

    private CameraManager cameraManager;

    private GameObject mainMenuButton;

    private void Awake()
    {
        /*
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(Instance);
        */

        canvas = GameObject.Find("Canvas");

        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;

        timerText = GameObject.Find("Timer Text").GetComponent<TextMeshProUGUI>();

        cameraManager = FindObjectOfType<CameraManager>();

        mainMenuButton = GameObject.Find("Main Menu Button");
    }

    private void Start()
    {
        timerText.SetText("");
    }

    private void Update()
    {
        if (canStart && !initializedPlayerScoreSize)
        {
            playerScore = new int[playerArray.Length];
            initializedPlayerScoreSize = true;
        }

        

        if (canStart && canCalculate)
        {
            if (gameTimer > 0f)
            {
                gameTimer -= Time.deltaTime;
                timerText.SetText(Mathf.RoundToInt(gameTimer).ToString());
            }
            else
            {
                CalculateWin();
            }
        }

        if (isPerformingWin)
        {
            mainMenuButton.SetActive(true);
        }
        else
        {
            mainMenuButton.SetActive(false);
        }
    }

    public void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
     
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
    }

    public void PlayerJoin()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Player");

        for(int i = 0; i < playerArray.Length; i++)
        {
            playerArray[i].GetComponent<TileColorChanger>().playerNum = i;
        }
    }

    public void PlayerLeave()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Player");
    }

    private void CalculateWin()
    {
        canCalculate = false;
        isPerformingWin = true;

        GameObject _stageScene = GameObject.Find("Stage Scene");
        Tile[] tiles = GameObject.FindObjectsOfType<Tile>();

        // Update player scores
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].GetComponent<Tile>().lastPlayer >= 0)
            {
                playerScore[tiles[i].GetComponent<Tile>().lastPlayer] += 1;
            }
        }

        // Convert playerScore to a List and sort in descending order
        List<int> sortedScores = new List<int>(playerScore);
        sortedScores.Sort((a, b) => b.CompareTo(a));  // Sort in descending order

        // Create a list to hold player indices in sorted order
        List<int> sortedPlayerIndices = new List<int>();
        foreach (int score in sortedScores)
        {
            for (int i = 0; i < playerScore.Length; i++)
            {
                if (playerScore[i] == score && !sortedPlayerIndices.Contains(i))
                {
                    sortedPlayerIndices.Add(i);
                    break;
                }
            }
        }

        // Set winner (1st place)
        int _winner = sortedPlayerIndices[0];
        GameObject _winnerObject = playerArray[_winner].gameObject;
        TileColorChanger _winnerTileColorChanger = _winnerObject.GetComponent<TileColorChanger>();

        // Win text
        timerText.SetText("Player " + (_winner + 1).ToString() + " won");
        timerText.color = _winnerTileColorChanger.colors[_winnerTileColorChanger.colorSelected];
        timerText.alpha = 255;
        timerText.outlineColor = Color.black;
        timerText.outlineWidth = 0.25f;

        // Animations for winner
        Animator _winnerAnimator = _winnerObject.GetComponentInChildren<Animator>();
        _winnerAnimator.SetInteger("RandomWinAnimation", Random.Range(1, 2));

        // Set the winner's position (1st place)
        foreach (Transform _child in _stageScene.transform)
        {
            if (_child.name == "1st Player Position")
            {
                _winnerObject.transform.position = _child.transform.position;
            }
        }

        // Set positions for 2nd, 3rd, and 4th place players
        for (int i = 1; i < sortedPlayerIndices.Count && i < 4; i++)
        {
            int playerIndex = sortedPlayerIndices[i];
            GameObject playerObject = playerArray[playerIndex].gameObject;

            // Find the position name based on the place (2nd, 3rd, 4th)
            string positionName = $"{(i + 1)}{GetSuffix(i + 1)} Player Position";
            foreach (Transform _child in _stageScene.transform)
            {
                if (_child.name == positionName)
                {
                    playerObject.transform.position = _child.transform.position;
                }
            }
        }

        cameraManager.ChangeCamera(cameraManager.mainCam, cameraManager.winCam);
    }

    // Helper function to get the suffix for position numbers (1st, 2nd, 3rd, 4th)
    private string GetSuffix(int number)
    {
        if (number == 1) return "st";
        else if (number == 2) return "nd";
        else if (number == 3) return "rd";
        else return "th";
    }

}