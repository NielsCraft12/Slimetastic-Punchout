using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Scene currentScene;
    private string currentSceneName;

    private string activeMenu = "Main Menu"; //activeMenu needs to be the menu you want to be active at the moment

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(Instance);
    }

    private void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;

        MenuLogic();
    }

    private void MenuLogic() 
    {
        GameObject _canvas = GameObject.Find("Canvas");

        foreach (Transform _child in _canvas.transform)
        {
            _child.gameObject.SetActive(false);

            if (_child.gameObject.name == activeMenu)
            {
                _child.gameObject.SetActive(true);
            }
        }
    }

    public void ChangeMenu(string _menu)
    {
        activeMenu = _menu;
    }

    public void ChangeScene(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }
}