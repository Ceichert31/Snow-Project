using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates UI text for slider value
/// </summary>
public class SliderUpdateText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI updateText;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(UpdateText);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(UpdateText);
    }

    private void UpdateText(float num)
    {
        updateText.text = Mathf.RoundToInt(num).ToString();
    }
}
