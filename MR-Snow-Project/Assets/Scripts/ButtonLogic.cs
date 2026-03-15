using UnityEngine;

/// <summary>
/// Handles all method calls for button interactions
/// </summary>
public class ButtonLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsMenu;

    public void Quit()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        //Open new worldspace menu
        //Possible settings:
        //Snow fall rate
        //Toggle snow

        settingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
