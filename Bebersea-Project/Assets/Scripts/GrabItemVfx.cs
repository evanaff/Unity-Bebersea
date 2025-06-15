using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class GrabVFXTrigger : MonoBehaviour
{
    public GameObject grabVFXPrefab; // Assign di Inspector

    private XRBaseInteractor interactor;

    private void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
    }

    private void OnEnable()
    {
        interactor.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        GameObject grabbedObject = args.interactableObject.transform.gameObject;

        if (grabbedObject.CompareTag("Trash"))
        {
            // Instantiate VFX at the object's position
            GameObject vfx = Instantiate(grabVFXPrefab, grabbedObject.transform.position, Quaternion.identity);

            // Optional: Auto-destroy after duration
            Destroy(vfx, 2f);
        }
    }
}
