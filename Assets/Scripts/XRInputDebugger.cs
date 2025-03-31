using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRInputDebugger : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _debugText;

    [SerializeField]
    private TextMeshProUGUI _debugTextValue;

    public InputActionReference actionReference;
    public InputActionReference actionValueReference;

    private void OnEnable()
    {
        if (actionReference != null)
        {
            actionReference.action.performed += OnInputPerformed;
            actionReference.action.canceled += OnInputCanceled;
            actionReference.action.started += OnInputStarted;
        }
    }

    private void OnDisable()
    {
        if (actionReference != null)
        {
            actionReference.action.performed -= OnInputPerformed;
            actionReference.action.canceled -= OnInputCanceled;
            actionReference.action.started -= OnInputStarted;
        }
    }

    private void Update()
    {
        var value = actionValueReference.action.ReadValue<float>();
        _debugTextValue.text = "" + value;
    }

    private void OnInputPerformed(InputAction.CallbackContext context)
    {
        _debugText.text = $"Performed: {context.control.name}, Path: {context.control.path}";
    }

    private void OnInputStarted(InputAction.CallbackContext context)
    {
        _debugText.text = $"Started: {context.control.name}, Path: {context.control.path}";
    }

    private void OnInputCanceled(InputAction.CallbackContext context)
    {
        _debugText.text = $"Canceled: {context.control.name}, Path: {context.control.path}";
    }
}
