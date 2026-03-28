using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles menu opening and closing
/// </summary>
public class MenuController : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The game object that holds the menu UI")]
    [SerializeField]
    private GameObject menuObject;

    [Tooltip("The speed the menu opens and closes at")]
    [SerializeField]
    private float menuAnimSpeed = 0.5f;

    [Tooltip("The menu animation easing function")]
    [SerializeField]
    private Ease menuEase;

    private bool isMenuOpen;

    private Tween currentTween;

    [ContextMenu("Toggle Menu")]
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        currentTween.Kill();

        currentTween = menuObject
            .transform.DOScaleY(isMenuOpen ? 1 : 0, menuAnimSpeed)
            .SetEase(menuEase);
    }
}
