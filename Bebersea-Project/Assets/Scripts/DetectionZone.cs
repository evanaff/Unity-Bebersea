using UnityEngine;
using UnityEngine.SceneManagement;

public class SubmarineDetectionZone : MonoBehaviour
{
    public GlowTarget[] glowTargets;
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("FakeSub")) return;

        foreach (var glow in glowTargets)
            glow.EnableGlow();

        GlowTarget.RegisterAction(() =>
        {
            SceneManager.LoadScene(sceneToLoad);
        });
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("FakeSub")) return;

        foreach (var glow in glowTargets)
            glow.DisableGlow();

        GlowTarget.ClearAction();
    }
}
