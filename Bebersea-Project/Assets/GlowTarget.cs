using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public class GlowTarget : MonoBehaviour
{
    public Renderer targetRenderer;
    public Color glowColor = Color.cyan;

    private Material _material;
    private Color _originalEmission;
    private static System.Action onGlowTargetSelected;

    private void Awake()
    {
        _material = targetRenderer.material;
        _originalEmission = _material.GetColor("_EmissionColor");
        _material.SetColor("_EmissionColor", Color.black);

        var interactable = GetComponent<XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelected);
    }

    public void EnableGlow()
    {
        _material.EnableKeyword("_EMISSION");
        _material.SetColor("_EmissionColor", glowColor);
    }

    public void DisableGlow()
    {
        _material.SetColor("_EmissionColor", Color.black);
    }

    public static void RegisterAction(System.Action callback)
    {
        onGlowTargetSelected = callback;
    }

    public static void ClearAction()
    {
        onGlowTargetSelected = null;
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        onGlowTargetSelected?.Invoke();
    }
}
