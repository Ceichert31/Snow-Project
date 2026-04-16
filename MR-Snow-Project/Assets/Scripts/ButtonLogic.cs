using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles all method calls for button interactions
/// </summary>
public class ButtonLogic : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private GameObject mainMenu;

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}