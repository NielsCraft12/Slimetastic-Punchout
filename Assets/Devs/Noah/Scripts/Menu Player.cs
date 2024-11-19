using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
    private MenuManager menuManager;
    private RectTransform rectTransform;
    [SerializeField]
    private List<GameObject> PlayerMenuChilds = new List<GameObject>();

    UIControls controls;
    private InputAction menu;

    [SerializeField]
    private int selected;

    private void Awake()
    {
        controls = new UIControls();
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
        rectTransform = GetComponent<RectTransform>();


        transform.SetParent(menuManager.playerSelections[0].transform, false);


        foreach (Transform playerMenuChilds in menuManager.playerSelections[0].transform)
        {
            if (playerMenuChilds.gameObject.name == "Cosmetic Selection" || playerMenuChilds.gameObject.name == "Color Selection" || playerMenuChilds.gameObject.name == "Ready Selection")
            {
                PlayerMenuChilds.Add(playerMenuChilds.gameObject);
            }
        }


        menuManager.playerSelections.RemoveAt(0);
        rectTransform.localPosition = GameObject.Find("Cosmetic Selection").GetComponent<RectTransform>().localPosition;
    }

    private void OnEnable()
    {
        menu = controls.Menus.Move;
        menu.Enable();
        menu.performed += NavigateMenu;
    }
    private void OnDisable()
    {
        menu.Disable();
    }

    private void Update()
    {
     

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Image>().color = Color.red;
        Debug.Log(collision.gameObject);

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Image>().color = Color.white;
    }

    private void NavigateMenu(InputAction.CallbackContext context)
    {
        Vector2 moveDir = menu.ReadValue<Vector2>();
        Debug.Log(moveDir);


        if (moveDir == new Vector2(0, 1))
        {
            selected--;
        }
        else if (moveDir == new Vector2(0, -1))
        {
            selected++;
        }
        if (selected > PlayerMenuChilds.Count -1)
        {
            selected = 0;
        } else if (selected < 0) {
            selected = PlayerMenuChilds.Count -1;
        }

        rectTransform.localPosition = PlayerMenuChilds[selected].transform.localPosition;

    }
}
