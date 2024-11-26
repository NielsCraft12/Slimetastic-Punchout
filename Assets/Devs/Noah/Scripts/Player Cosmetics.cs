using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCosmetics : MonoBehaviour
{
    [SerializeField] private List<GameObject> cosmeticModels = new List<GameObject>(); // The list where all the cosmetic models will be in

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
                SetCosmetic("Cube");
                break;

            case Cosmetics.Fez:
                SetCosmetic("BEAN");
                break;

            case Cosmetics.Halo:
                SetCosmetic("Halo");
                break;
            case Cosmetics.PartyHat:
                SetCosmetic("PartyHat");
                break;
            case Cosmetics.TopHatHorns:
                SetCosmetic("TopHatHorns");
                break;
            case Cosmetics.TopHat:
                SetCosmetic("TopHat");
                break;
            case Cosmetics.TrafficCone:
                SetCosmetic("TrafficCone");
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
