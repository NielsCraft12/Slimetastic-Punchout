using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCosmetics : MonoBehaviour
{
    [SerializeField] private List<GameObject> cosmeticModels = new List<GameObject>(); // The list where all the cosmetic models will be in


    public int cosmeticIndex = 0;

    public enum Cosmetics
    {
        None,
        Axe,
        Fez,
        Halo,
        PartyHat,
        TopHatHorns,
        TopHat,
        TrafficCone,
        Bootleg
    }

    [SerializeField] public Cosmetics cosmetic;

    private void Start()
    {
        foreach (Transform _child in transform)
        {
            cosmeticModels.Add(_child.gameObject);
        }

        SetCosmetic("None");
    }

    private void Update()
    {
        switch (cosmetic)
        {
            case Cosmetics.None:
                SetCosmetic("None");
                break;

            case Cosmetics.Axe:
                SetCosmetic("P_Hand Axe");
                break;

            case Cosmetics.Fez:
                SetCosmetic("P_Fez");
                break;

            case Cosmetics.Halo:
                SetCosmetic("P_Halo");
                break;
            case Cosmetics.PartyHat:
                SetCosmetic("P_Party hat");
                break;
            case Cosmetics.TopHatHorns:
                SetCosmetic("P_Top hat with horns");
                break;
            case Cosmetics.TopHat:
                SetCosmetic("P_Top Hat");
                break;
            case Cosmetics.TrafficCone:
                SetCosmetic("P_Traffic cone");
                break;
            case Cosmetics.Bootleg:
                SetCosmetic("Bootleg");
                break;

            default:
                SetCosmetic("None");
                break;
        }
    }

    private void SetCosmetic(string _cosmetic)
    {
        foreach (GameObject _cosmeticModel in cosmeticModels)
        {
            if (_cosmeticModel.name == _cosmetic)
            {
                _cosmeticModel.gameObject.SetActive(true);
            }
            else
            {
                _cosmeticModel.gameObject.SetActive(false);
            }
        }
    }
}
