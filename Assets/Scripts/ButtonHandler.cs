using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField]
    private Button _button;

    [SerializeField]
    private TextMeshProUGUI _counterText;

    private int _counter = 0;

    private void Awake()
    {
        UpdateCounter();
        _button
            .onClick
            .AddListener(() =>
            {
                _counter++;
                UpdateCounter();
            });
    }

    private void UpdateCounter()
    {
        _counterText.text = string.Format("Pressed: {0}", _counter);
    }
}
