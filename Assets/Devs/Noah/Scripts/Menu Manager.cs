using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public List<GameObject> playerSelections = new List<GameObject>();
    [SerializeField] private GameObject playerSelect;

    public List<GameObject> menuSelections = new List<GameObject>();


    private void Awake()
    {
        playerSelect = GameObject.Find("Player Select");

        for (int i = 1; i <= 4; i++)
        {
          GameObject playerMenu = GameObject.Find("Player " + i);
            playerSelections.Add(playerMenu);
        }


    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"Player {playerInput.playerIndex} joined with device: {playerInput.devices[0].name}");
        // Additional setup for the player (e.g., camera or UI).
    }
}