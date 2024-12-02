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

    [SerializeField] private bool canStart = false;

    private Transform tileManager;

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

        tileManager = GameObject.Find("TileManager").GetComponent<Transform>();
    }

    private void Start()
    {
        playerScore = new int[4] {0, 0, 0, 0};
    }

    private void Update()
    {
        if (canStart && canCalculate)
        {
            if (gameTimer > 0f)
            {
                gameTimer -= Time.deltaTime;
            }
            else
            {
                CalculateWin();
            }

            timerText.SetText(Mathf.RoundToInt(gameTimer).ToString());
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

        Tile[] tiles = GameObject.FindObjectsOfType<Tile>();

        for(int i = 0; i < tiles.Length; i++)
        {
            
            if(tiles[i].GetComponent<Tile>().lastPlayer >= 0)
            {
                Debug.Log(tiles[i].GetComponent<Tile>().lastPlayer);
                playerScore[tiles[i].GetComponent<Tile>().lastPlayer]+= 1;
            }
        }

        

        timerText.SetText("frltne");
    }
}