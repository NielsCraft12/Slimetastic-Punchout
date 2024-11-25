using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Podium : MonoBehaviour
{
    [SerializeField] private string[] randomText;
    [SerializeField] private TextMeshProUGUI podiumText;

    private void Awake()
    {
        podiumText.SetText(randomText[Random.Range(0,randomText.Length)]);
    }
}
