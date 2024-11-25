using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
    private MenuManager menuManager;
    private RectTransform rectTransform;

    [SerializeField]
    private List<GameObject> PlayerMenuChilds = new List<GameObject>();

    [SerializeField]
    private List<GameObject> images = new List<GameObject>();

    [SerializeField]
    private int selected;

    [SerializeField]
    private int colorSelected = -1;

    [SerializeField]
    private int cosmeticSelected;

    private GameObject Outline;
    private GameObject playerIcon;

    [SerializeField]
    private List<GameObject> Locks = new List<GameObject>();

    public bool isReady;

    private void Awake()
    {
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
        rectTransform = GetComponent<RectTransform>();

        menuManager.playerSelectors.Add(this.gameObject);
        transform.SetParent(menuManager.playerSelections[0].transform, false);

        foreach (Transform playerMenuChilds in menuManager.playerSelections[0].transform)
        {
            if (playerMenuChilds.gameObject.name == "Cosmetic Selection"
                || playerMenuChilds.gameObject.name == "Color Selection"
                || playerMenuChilds.gameObject.name == "Ready Selection")
            {
                PlayerMenuChilds.Add(playerMenuChilds.gameObject);
            }
            else if (playerMenuChilds.gameObject.name == "Cosmatic Image"
                || playerMenuChilds.gameObject.name == "Color Image")
            {
                images.Add(playerMenuChilds.gameObject);
            }
            else if (playerMenuChilds.gameObject.name == "Outline")
            {
                Outline = playerMenuChilds.gameObject;
            }
            else if (playerMenuChilds.gameObject.name == "Player Icon")
            {
                playerIcon = playerMenuChilds.gameObject;
            }
            else if (playerMenuChilds.gameObject.name == "Lock")
            {
                playerMenuChilds.gameObject.GetComponent<Image>().enabled = false;
                Locks.Add(playerMenuChilds.gameObject);
            }
        }

        menuManager.playerSelections.RemoveAt(0);
        rectTransform.localPosition = GameObject.Find("Cosmetic Selection").GetComponent<RectTransform>().localPosition;

        menuManager.takenColors.Add(
            new ColorTracker { PlayerName = transform.parent.name, Color = colorSelected }
        );
        ChooseColor();

        images[1].GetComponent<Image>().color = menuManager.colors[colorSelected];
        Outline.GetComponent<Image>().color = menuManager.colors[colorSelected];
        playerIcon.GetComponent<Image>().color = menuManager.colors[colorSelected];

        menuManager.GetPlayers();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ArrowUi"))
        {
            ArrowSelect(collision);
        }
        collision.GetComponent<Image>().color = menuManager.colors[colorSelected];
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<Image>().color = Color.white;
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (selected == 2 && isReady == false)
        {
            isReady = true;
        }
        else if (selected == 2 && isReady == true)
        {
            isReady = false;
        }
    }

    public void NavigateMenu(InputAction.CallbackContext context)
    {
        Vector2 moveDir = context.ReadValue<Vector2>();

        if (moveDir == new Vector2(0, 1))
        {
            selected++;
        }
        else if (moveDir == new Vector2(0, -1))
        {
            selected--;
        }
        else if (moveDir == new Vector2(-1, 0) && selected != 2)
        {
            StartCoroutine(SidetoSide(-50));
            return;
        }
        else if (moveDir == new Vector2(1, 0) && selected != 2)
        {
            StartCoroutine(SidetoSide(50));
            return;
        }

        if (selected > PlayerMenuChilds.Count - 1)
        {
            selected = 0;
        }
        else if (selected < 0)
        {
            selected = PlayerMenuChilds.Count - 1;
        }
        rectTransform.localPosition = PlayerMenuChilds[selected].transform.localPosition;
    }

    IEnumerator SidetoSide(int jumpStrength)
    {
        rectTransform.transform.localPosition = new Vector2(
            jumpStrength,
            rectTransform.localPosition.y
        );
        yield return new WaitForSeconds(0.1f);
        rectTransform.localPosition = PlayerMenuChilds[selected].transform.localPosition;
    }

    private void ArrowSelect(Collider2D collision)
    {
        if (selected == 0)
        {
            if (collision.gameObject.name == "Right Arrow")
            {
                cosmeticSelected++;
            }
            else if (collision.gameObject.name == "Left Arrow")
            {
                cosmeticSelected--;
            }
        }
        else if (selected == 1)
        {
            menuManager.takenColors.RemoveAll(s => s.PlayerName == transform.parent.name);

            if (collision.gameObject.name == "Right Arrow")
            {
                colorSelected++;
            }
            else if (collision.gameObject.name == "Left Arrow")
            {
                colorSelected--;
            }
            if (colorSelected > menuManager.colors.Count - 1)
            {
                colorSelected = 0;
            }
            else if (colorSelected < 0)
            {
                colorSelected = menuManager.colors.Count - 1;
            }
            CheckIfColorUsed();

            menuManager.takenColors.Add(
                new ColorTracker { PlayerName = transform.parent.name, Color = colorSelected }
            );
            PlayerMenuChilds[selected].GetComponent<Image>().color = menuManager.colors[
                colorSelected
            ];
            Outline.GetComponent<Image>().color = menuManager.colors[colorSelected];
            playerIcon.GetComponent<Image>().color = menuManager.colors[colorSelected];
            images[1].GetComponent<Image>().color = menuManager.colors[colorSelected];
        }
    }

    private void Update()
    {
        CheckIfColorUsed();
    }

    public void CheckIfColorUsed()
    {
        bool shouldShowLock = false;
        int myTakenColorsIndex = -1;

        for (int i = 0; i < menuManager.takenColors.Count; i++)
        {
            // Check if the color is taken by another player
            if (
                colorSelected == menuManager.takenColors[i].Color
                && menuManager.takenColors[i].PlayerName != transform.parent.name
                && menuManager.takenColors[i].first == true
            )
            {
                shouldShowLock = true;
            }
            else if (menuManager.takenColors[i].PlayerName == transform.parent.name)
            {
                myTakenColorsIndex = i;
            }
        }
        if (myTakenColorsIndex != -1)
        {
            menuManager.takenColors[myTakenColorsIndex].first = !shouldShowLock;
        }

        foreach (GameObject LockIcon in Locks)
        {
            LockIcon.gameObject.GetComponent<Image>().enabled = shouldShowLock;
        }
    }

    private void ChooseColor()
    {
        while (colorSelected == -1)
        {
            int randomColor = Random.Range(0, menuManager.colors.Count);

            for (int i = 0; i < menuManager.takenColors.Count; i++)
            {
                if (randomColor == menuManager.takenColors[i].Color)
                {
                    colorSelected = -1;
                    break;
                }
                else
                {
                    colorSelected = randomColor;
                }
            }
        }

        menuManager.takenColors.RemoveAll(s => s.PlayerName == transform.parent.name);
        menuManager.takenColors.Add(
  new ColorTracker { PlayerName = transform.parent.name, Color = colorSelected }
);
    }
}
