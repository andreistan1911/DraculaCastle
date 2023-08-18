using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private Button continueButton;

    private void Start()
    {
        GlobalValues.LogAllPrefs();
        if (PlayerPrefs.GetInt("Level") == 0)
        {
            // Disable 'Continue Game' button if there are no saved checkpoints
            continueButton = GameObject.Find("Continue Game").GetComponent<Button>();
            continueButton.interactable = SaveSystem.HasSavedCheckpoints();
        }
    }

    public void NewGame()
    {
        GlobalValues.SaveGame(1, 0, 0, 0, 0, 0, 0, 0);

        // Load your first level scene
        SceneManager.LoadScene("Scenes/New_Game");
    }

    public void ContinueGame()
    {
        GlobalValues.continueLoading = true;
        GlobalValues.totalTImer = PlayerPrefs.GetFloat("Timer");

        // Load your game scene
        SceneManager.LoadScene("Scenes/Level_" + PlayerPrefs.GetInt("Level"));
    }

    public void ExitGame()
    {
        Application.Quit();

        // If you are in the Unity editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
