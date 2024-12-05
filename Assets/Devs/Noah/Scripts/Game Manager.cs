using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
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

        for(int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].GetComponent<Tile>().lastPlayer >= 0)
            {
                playerScore[tiles[i].GetComponent<Tile>().lastPlayer]+= 1;
            }
        }

        int[] _tempScore = playerScore;

        int _winScore = Mathf.Max(playerScore);

        int _winner = -1;

        GameObject _winnerObject;
        TileColorChanger _winnerTileColorChanger;

        for (int i = 0; i < _tempScore.Length; i++)
        {
            if (_tempScore[i] == _winScore)
            {
                if (_winner == -1)
                {
                    // Set winner
                    _winner = i;

                    _winnerObject = playerArray[i].gameObject;
                    _winnerTileColorChanger = _winnerObject.GetComponent<TileColorChanger>(); 

                    // Win text
                    timerText.SetText("Player " + (_winner + 1).ToString() + " won");
                    timerText.color = _winnerTileColorChanger.colors[_winnerTileColorChanger.colorSelected];
                    timerText.alpha = 255;
                    timerText.outlineColor = Color.black;
                    timerText.outlineWidth = 0.25f;

                    // Animations
                    Animator _winnerAnimator = _winnerObject.GetComponentInChildren<Animator>();
                    _winnerAnimator.SetInteger("RandomWinAnimation", Random.Range(1,2));
                }
            }

            // Set correct positions
            foreach(Transform _child in _stageScene.transform)
            {
                if (_child.name == "1st Player Position")
                {
                    
                }
            }
        }

        cameraManager.ChangeCamera(cameraManager.mainCam, cameraManager.winCam);
    }
}