using UnityEngine;

/// <summary>
/// Handles all method calls for button interactions
/// </summary>
public class ButtonLogic : MonoBehaviour
{
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
    }
}
