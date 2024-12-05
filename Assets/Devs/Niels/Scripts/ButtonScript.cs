using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// Handles button functionality including scene transitions, sounds, and quit game operations

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    // Sound effect names for button interactions
    [SerializeField] private string hoverSoundName = "hoverSound";
    [SerializeField] private string clickSoundName = "clickSound";


    /// Initializes button click listener and plays background music
    private void Start()
    {
        // Set up click sound for UI Button if present
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => AudioManager.instance.Play(clickSoundName));
        }

        // Start background music only if it's not already playing
        if (!AudioManager.instance.IsPlaying("MenuMusic"))
        {
            AudioManager.instance.Play("MenuMusic");
        }
    }

    /// Plays hover sound when pointer enters button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover detected");
        AudioManager.instance.Play(hoverSoundName);
    }

    /// Plays click sound when button is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click detected");
        AudioManager.instance.Play(clickSoundName);
    }

    /// Loads a new scene by name
    /// <param name="sceneName">Name of the scene to load</param>
    public void GoToScene(string sceneName)
    {
        if (sceneName == "Play")
        {
            AudioManager.instance.Stop("MenuMusic");
            AudioManager.instance.Play("BackgroundMusic");
        };
        SceneManager.LoadScene(sceneName);
    }

    /// Handles game exit functionality for different platforms
    public void QuitGame()
    {
        // Debug logging for development builds
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
        // Platform specific quit behavior
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#elif (UNITY_WEBGL)
        Application.OpenURL("itch url ");
#endif
    }
}
