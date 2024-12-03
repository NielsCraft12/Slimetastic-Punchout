using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
    }

    private void Start()
    {
        playerScore = new int[4] {0, 0, 0, 0};

        timerText.SetText("");
    }

    private void Update()
    {
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

        Tile[] tiles = GameObject.FindObjectsOfType<Tile>();

        for(int i = 0; i < tiles.Length; i++)
        {
            
            if(tiles[i].GetComponent<Tile>().lastPlayer >= 0)
            {
                Debug.Log(tiles[i].GetComponent<Tile>().lastPlayer);
                playerScore[tiles[i].GetComponent<Tile>().lastPlayer]+= 1;
            }
        }

        int _winScore = Mathf.Max(playerScore);

        int _winner;

        for (int i = 0; i < playerScore.Length; i++)
        {
            if (playerScore[i] == _winScore)
            {
                _winner = i;
                Debug.Log(i);
                GameObject _winnerObject = playerArray[i].gameObject;
                TileColorChanger _winnerTileColorChanger = _winnerObject.GetComponent<TileColorChanger>();

                timerText.SetText("Player " + _winner.ToString() + " won");
                timerText.color = _winnerTileColorChanger.colors[_winnerTileColorChanger.colorSelected];
                timerText.alpha = 255;
                timerText.outlineColor = Color.black;
                timerText.outlineWidth = 0.25f;

                // Win animation
                /*
                Animator _winAnimator = playerArray[i].GetComponentInChildren<Animator>();
                _winAnimator.SetBool("IsPerformingWin", true);
                _winAnimator.SetInteger("RandomWinAnimation", Random.Range(0,1));
                */
            }
            else
            {
                // Win animation
                /*Animator _winAnimator = playerArray[i].GetComponentInChildren<Animator>();
                _winAnimator.SetBool("IsPerformingWin", true);
                _winAnimator.SetInteger("RandomWinAnimation", Random.Range(0,1));
                */
            }
        }
    }
}