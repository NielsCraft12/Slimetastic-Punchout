using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    private GameObject player;
    private GameObject cosmeticParent;

    [SerializeField]
    private GameObject PlayerBody;
    [SerializeField]
    private GameObject PlayerEyes;
    [SerializeField]
    private TextMeshProUGUI ReadyText;
    public bool isReady;
    [SerializeField]
    private GameObject PlayablePlayer;


    private TileColorChanger tileColorChanger;

    private void Awake()
    {
        // Initialize components and setup the player menu
        InitializeComponents();
        AssignPlayerMenuChildren();
        AssignPlayerComponents();
        InitializePlayerSelection();
        InitializeColorSelection();
        UpdateUI();
    }

    private void InitializeComponents()
    {
        // Find and assign the MenuManager and RectTransform components
        menuManager = GameObject.Find("Menu Manager").GetComponent<MenuManager>();
        rectTransform = GetComponent<RectTransform>();
        // Add this player selector to the MenuManager
        menuManager.playerSelectors.Add(this.gameObject);
        // Set the parent of this transform to the first player selection


        transform.SetParent(menuManager.playerSelections[0].transform, false);

        menuManager.slimes.Add(transform.GetChild(0).gameObject);
        // transform.GetChild(0).GetComponent<PlayerInput>().enabled = false;
        // transform.GetChild(0).GetComponent<TileColorChanger>().enabled = false;
        // transform.GetChild(0).GetComponent<PlayerController>().enabled = false;
        // transform.GetChild(0).GetComponent<PlayerAttack>().enabled = false;
        tileColorChanger = transform.GetChild(0).GetComponent<TileColorChanger>();
        PlayablePlayer = transform.GetChild(0).gameObject;
        transform.GetChild(0).transform.SetParent(menuManager.playersParent.transform, false);

        // Add debug logs
        Debug.Log("Initialized components for MenuPlayer");
    }

    private void AssignPlayerMenuChildren()
    {
        // Iterate through the player menu children and assign them to appropriate lists or variables
        foreach (Transform playerMenuChild in menuManager.playerSelections[0].transform)
        {
            switch (playerMenuChild.gameObject.name)
            {
                case "Cosmetic Selection":
                case "Color Selection":
                case "Ready Selection":
                    PlayerMenuChilds.Add(playerMenuChild.gameObject);
                    break;
                case "Cosmatic Image":
                case "Color Image":
                    images.Add(playerMenuChild.gameObject);
                    break;
                case "Outline":
                    Outline = playerMenuChild.gameObject;
                    break;
                case "Player Icon":
                    playerIcon = playerMenuChild.gameObject;
                    break;
                case "Lock":
                    playerMenuChild.gameObject.GetComponent<Image>().enabled = false;
                    Locks.Add(playerMenuChild.gameObject);
                    break;
                case "Ready":
                    ReadyText = playerMenuChild.GetComponent<TextMeshProUGUI>();
                    ReadyText.text = "Not Ready...";
                    break;
            }

            // Assign player object based on parent name
            if (playerMenuChild.gameObject.name == "Player" + transform.parent.name)
            {
                player = playerMenuChild.gameObject;
            }
        }
    }

    private void AssignPlayerComponents()
    {
        // Find and assign cosmetic parent, player body, and player eyes
        foreach (Transform child in player.transform)
        {
            if (child.gameObject.name == "Cosmetic Parent")
            {
                cosmeticParent = child.gameObject;
            }

            if (child.gameObject.name == "P_Red Slime")
            {
                AssignSlimeComponents(child);
            }
        }
    }

    private void AssignSlimeComponents(Transform slimeTransform)
    {
        // Assign the body and eyes of the slime
        foreach (Transform subChild in slimeTransform)
        {
            if (subChild.gameObject.name == "Body")
            {
                PlayerBody = subChild.Find("Slime Model").gameObject;
            }

            if (subChild.gameObject.name == "Eyes")
            {
                PlayerEyes = subChild.Find("Slime Eyes").gameObject;
            }
        }
    }

    private void InitializePlayerSelection()
    {
        // Remove the first player selection and set initial position
        menuManager.playerSelections.RemoveAt(0);
        rectTransform.localPosition = GameObject.Find("Cosmetic Selection").GetComponent<RectTransform>().localPosition;
    }

    private void InitializeColorSelection()
    {
        // Add initial color to taken colors and update UI
        menuManager.takenColors.Add(new ColorTracker { PlayerName = transform.parent.name, Color = colorSelected });
        ChooseColor();
    }

    private void UpdateUI()
    {
        // Update the color and cosmetic selection, and set the ready text
        UpdateColor();
        menuManager.GetPlayers();
        UpdateCosmetic();
        ReadyText.text = "Not Ready...";
        ReadyText.color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle arrow UI collision
        if (collision.gameObject.CompareTag("ArrowUi"))
        {
            ArrowSelect(collision);
        }
        collision.GetComponent<Image>().color = menuManager.colors[colorSelected];
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset color on exit
        collision.GetComponent<Image>().color = Color.white;
    }

    public void Select(InputAction.CallbackContext context)
    {
        // Handle selection input
        int GetIndex = menuManager.takenColors.FindIndex(colorTracker => colorTracker.PlayerName == transform.parent.name);
        if (selected == 2 && menuManager.takenColors[GetIndex].first)
        {
            isReady = !isReady;
        }
        ReadyChanger();
    }

    private void ReadyChanger()
    {
        // Update ready status and text
        int GetIndex = menuManager.takenColors.FindIndex(colorTracker => colorTracker.PlayerName == transform.parent.name);
        if (selected == 2)
        {
            if (isReady && menuManager.takenColors[GetIndex].first)
            {
                ReadyText.text = "Ready";
                ReadyText.color = Color.green;
            }
            else
            {
                ReadyText.text = "Not Ready...";
                ReadyText.color = Color.red;
                isReady = false;
            }
        }
    }

    public void NavigateMenu(InputAction.CallbackContext context)
    {
        // Handle menu navigation input
        Vector2 moveDir = context.ReadValue<Vector2>();

        if (moveDir == new Vector2(0, 1))
        {
            isReady = false;
            selected++;
        }
        else if (moveDir == new Vector2(0, -1))
        {
            isReady = false;
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
        UpdateReadyText();
    }

    IEnumerator SidetoSide(int jumpStrength)
    {
        // Handle side-to-side movement
        rectTransform.transform.localPosition = new Vector2(jumpStrength, rectTransform.localPosition.y);
        yield return new WaitForSeconds(0.1f);
        rectTransform.localPosition = PlayerMenuChilds[selected].transform.localPosition;
    }

    private void ArrowSelect(Collider2D collision)
    {
        // Handle arrow selection for cosmetics and colors
        if (selected == 0)
        {
            isReady = false;
            cosmeticSelected += collision.gameObject.name == "Right Arrow" ? 1 : -1;

            if (cosmeticSelected > menuManager.Cosmetics.Count - 1)
            {
                cosmeticSelected = 0;
            }
            else if (cosmeticSelected < 0)
            {
                cosmeticSelected = menuManager.Cosmetics.Count - 1;
            }
        }
        else if (selected == 1)
        {
            menuManager.takenColors.RemoveAll(s => s.PlayerName == transform.parent.name);

            isReady = false;
            colorSelected += collision.gameObject.name == "Right Arrow" ? 1 : -1;

            if (colorSelected > menuManager.colors.Count - 1)
            {
                colorSelected = 0;
            }
            else if (colorSelected < 0)
            {
                colorSelected = menuManager.colors.Count - 1;
            }
            CheckIfColorUsed();

            menuManager.takenColors.Add(new ColorTracker { PlayerName = transform.parent.name, Color = colorSelected });
            PlayerMenuChilds[selected].GetComponent<Image>().color = menuManager.colors[colorSelected];

            UpdateColor();
        }

        if (selected == 0)
        {
            images[0].GetComponent<Image>().sprite = menuManager.Cosmetics[cosmeticSelected];
            UpdateCosmetic();
        }
        UpdateReadyText();
    }

    private void Update()
    {
        // Check if color is used every frame
        CheckIfColorUsed();
    }

    public void CheckIfColorUsed()
    {
        // Check if the selected color is already taken by another player
        bool shouldShowLock = false;
        int myTakenColorsIndex = -1;

        for (int i = 0; i < menuManager.takenColors.Count; i++)
        {
            if (colorSelected == menuManager.takenColors[i].Color && menuManager.takenColors[i].PlayerName != transform.parent.name && menuManager.takenColors[i].first)
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
        // Choose a random color that is not already taken
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
        menuManager.takenColors.Add(new ColorTracker { PlayerName = transform.parent.name, Color = colorSelected });
    }

    private void UpdateColor()
    {
        // Update the color of various UI elements and player materials
        Outline.GetComponent<Image>().color = menuManager.colors[colorSelected];
        playerIcon.GetComponent<Image>().color = menuManager.colors[colorSelected];
        images[1].GetComponent<Image>().color = menuManager.colors[colorSelected];
        PlayerBody.GetComponent<Renderer>().material = menuManager.playerColors[colorSelected];
        PlayerEyes.GetComponent<Renderer>().material = menuManager.playerColors[colorSelected];

        // Update TileColorChanger's selected color
        if (tileColorChanger != null)
        {
            tileColorChanger.colorSelected = colorSelected;
            tileColorChanger.playerColors = menuManager.playerColors;
            tileColorChanger.colors = menuManager.colors;
        }
    }

    void UpdateCosmetic()
    {
        // Update the player's cosmetic selection
        PlayerCosmetics playerCosmetics = cosmeticParent.GetComponent<PlayerCosmetics>();
        playerCosmetics.cosmetic = (PlayerCosmetics.Cosmetics)cosmeticSelected;
        PlayerCosmetics playerCosmetics1 = PlayablePlayer.transform.GetChild(0).GetComponent<PlayerCosmetics>();
        //playerCosmetics1.cosmeticIndex = cosmeticSelected;
        playerCosmetics1.cosmetic = (PlayerCosmetics.Cosmetics)cosmeticSelected;
    }

    private void UpdateReadyText()
    {
        // Reset ready text to default
        ReadyText.text = "Not Ready...";
        ReadyText.color = Color.red;
    }







}
