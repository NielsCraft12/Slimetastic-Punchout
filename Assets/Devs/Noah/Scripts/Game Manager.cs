using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Scene currentScene;
    private string currentSceneName;

    private GameObject canvas;

    public GameObject[] playerArray;

    private PlayerInputManager playerInputManager;

    private float gameTimer = 120f;
    private TextMeshProUGUI timerText;

    [SerializeField] private bool canStart = false;

    private Transform tileManager;

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

        tileManager = GameObject.Find("Tile Manager").GetComponent<Transform>();
    }

    private void Update()
    {
        if (canStart)
        {
            if (gameTimer > 0f)
            {
                gameTimer -= Time.deltaTime;
            }
            else
            {
                CalculateWin();
            }

            timerText.SetText(gameTimer.ToString());
        }
        else
        {
            timerText.SetText("");
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
    }

    public void PlayerLeave()
    {
        playerArray = GameObject.FindGameObjectsWithTag("Player");
    }

    private void CalculateWin()
    {
        foreach(Transform _child in tileManager)
        {
            
        }
    }
}