using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles menu opening and closing
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("UI References")] [Tooltip("The game object that holds the menu UI")] [SerializeField]
    private GameObject menuObject;

    [Tooltip("The speed the menu opens and closes at")] [SerializeField]
    private float menuAnimSpeed = 0.5f;

    [Tooltip("The menu animation easing function")] [SerializeField]
    private Ease menuEase;

    [SerializeField] private float menuDistance = 5;

    [Tooltip("How far the player needs to be from the menu for it to relocate")] [SerializeField]
    private float relocateDistance = 10f;

    [Header("Player References")] [SerializeField]
    private Transform headTransform;

    private bool isMenuOpen;

    private Tween currentTween;

    void Update()
    {
        if (!isMenuOpen) return;

        if (Vector3.Distance(gameObject.transform.position, headTransform.position) > relocateDistance)
        {
            isMenuOpen = false;
        }

        gameObject.transform.LookAt(new Vector3(headTransform.position.x, gameObject.transform.position.y,
            headTransform.position.z));
    }

    [ContextMenu("Toggle Menu")]
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        //Set UI to player head forward
        gameObject.transform.position = headTransform.position +
                                        new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized *
                                        menuDistance;

        currentTween.Kill();

        currentTween = menuObject
            .transform.DOScaleY(isMenuOpen ? 1 : 0, menuAnimSpeed)
            .SetEase(menuEase);
    }
}