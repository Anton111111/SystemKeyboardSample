using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class PinchStatus : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _statusText;

    [SerializeField]
    private XRInputButtonReader _handRightValueInput = new("Value");

    private void Update()
    {
        var performed = _handRightValueInput.ReadIsPerformed();

        var value = _handRightValueInput.ReadValue();
        _statusText.text = string.Format(
            "performed: {0} ('{1}' '{2}') value: {3:0.0}",
            _handRightValueInput.ReadIsPerformed(),
            _handRightValueInput.inputActionPerformed,
            _handRightValueInput.inputActionReferencePerformed,
            value
        );
    }
}
