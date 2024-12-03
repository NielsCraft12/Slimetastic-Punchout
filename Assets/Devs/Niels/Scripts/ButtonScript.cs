using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Add this

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private string hoverSoundName = "hoverSound";
    [SerializeField] private string clickSoundName = "clickSound";

    private void Start()
    {
        // Add click listener if this is a UI Button
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => AudioManager.instance.Play(clickSoundName));
        }

        AudioManager.instance.Play("BackgroundMusic");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover detected");
        AudioManager.instance.Play(hoverSoundName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click detected");
        AudioManager.instance.Play(clickSoundName);
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    public void QuitGame()
    {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
        Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
             Application.Quit();
#elif (UNITY_WEBGL)
             Application.OpenURL("itch url ");
#endif
    }

}
