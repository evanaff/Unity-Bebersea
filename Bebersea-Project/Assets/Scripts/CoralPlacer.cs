//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.Interactables;
//using UnityEngine.XR.Interaction.Toolkit.Interactors;
//using System.Collections.Generic;

//public class CoralPlanting : MonoBehaviour
//{
//    [Header("Referensi Coral & Kontrol")]
//    public GameObject coralPrefab;
//    public InputActionProperty triggerAction;
//    public XRBaseInteractor leftHandInteractor;

//    [Header("Pengaturan Preview")]
//    public float plantingDistance = 2f;
//    public float manualOffsetY = 0.5f;
//    public Material previewMaterial;
//    public Vector3 coralScale = Vector3.one * 1.5f;

//    private GameObject coralPreviewInstance;
//    private Quaternion previewRotation;
//    private Vector3 previewPosition;
//    private bool previewActiveLastFrame = false;

//    [Header("Batasan Penanaman")]
//    public int maxCoralCount = 5;
//    public float minDistanceBetweenCorals = 0.5f;

//    private int plantedCoralCount = 0;
//    private List<GameObject> plantedCorals = new List<GameObject>();

//    public CoralPlantingProgressTracker progressTracker;
//    public ProgressInfo progressInfo;
//    public int sceneLoad;

//    private void Start()
//    {
//        // Buat preview coral di scene (disembunyikan sampai dibutuhkan)
//        coralPreviewInstance = Instantiate(coralPrefab);
//        coralPreviewInstance.transform.localScale = coralScale;
//        coralPreviewInstance.SetActive(false);

//        // Matikan collider agar tidak mengganggu fisika
//        foreach (var col in coralPreviewInstance.GetComponentsInChildren<Collider>())
//        {
//            col.enabled = false;
//        }

//        // (Opsional) Ubah material preview menjadi transparan
//        if (previewMaterial != null)
//        {
//            foreach (var rend in coralPreviewInstance.GetComponentsInChildren<Renderer>())
//            {
//                rend.material = previewMaterial;
//            }
//        }
//    }

//    private void Update()
//    {
//        // Cek apakah tangan kiri sedang memegang coral
//        if (!IsLeftHandGrabbingCoral())
//        {
//            if (coralPreviewInstance.activeSelf)
//            {
//                coralPreviewInstance.SetActive(false);
//                previewActiveLastFrame = false;
//            }
//            return;
//        }

//        // Cek raycast dari tangan kanan ke area tanam
//        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, plantingDistance)
//            && hit.collider.CompareTag("PlantableArea"))
//        {
//            coralPreviewInstance.SetActive(true);
//            previewPosition = hit.point + new Vector3(0, manualOffsetY, 0);
//            coralPreviewInstance.transform.position = previewPosition;

//            if (!previewActiveLastFrame)
//            {
//                // Set rotasi random hanya saat pertama kali aktif
//                previewRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
//                coralPreviewInstance.transform.rotation = previewRotation;
//            }

//            previewActiveLastFrame = true;
//        }
//        else
//        {
//            coralPreviewInstance.SetActive(false);
//            previewActiveLastFrame = false;
//        }
//    }

//    private void OnEnable()
//    {
//        if (triggerAction.action != null)
//        {
//            triggerAction.action.Enable();
//            triggerAction.action.performed += OnTriggerPerformed;
//        }
//    }

//    private void OnDisable()
//    {
//        if (triggerAction.action != null)
//        {
//            triggerAction.action.performed -= OnTriggerPerformed;
//            triggerAction.action.Disable();
//        }
//    }
//    private void OnTriggerPerformed(InputAction.CallbackContext context)
//    {
//        if (!coralPreviewInstance.activeSelf)
//        {
//            Debug.Log("Preview tidak aktif, tidak menanam.");
//            return;
//        }

//        if (!IsLeftHandGrabbingCoral())
//        {
//            Debug.Log("Tangan kiri tidak memegang coral, tidak bisa menanam.");
//            return;
//        }

//        if (plantedCoralCount >= maxCoralCount)
//        {
//            Debug.Log("Batas maksimal coral sudah tercapai.");
//            return;
//        }

//        Vector3 finalPosition = coralPreviewInstance.transform.position;
//        Quaternion finalRotation = coralPreviewInstance.transform.rotation;

//        if (IsTooCloseToOtherCorals(finalPosition))
//        {
//            Debug.Log("Terlalu dekat dengan coral lain, tidak bisa menanam.");
//            return;
//        }

//        GameObject newCoral = Instantiate(coralPrefab, finalPosition, finalRotation);
//        newCoral.transform.localScale = coralScale;

//        plantedCorals.Add(newCoral);
//        plantedCoralCount++;
//        progressInfo.AddCoralProgress(sceneLoad, 1, maxCoralCount);
//        if (progressTracker != null)
//        {
//            progressTracker.IncrementCoral();
//        }

//        Debug.Log("Coral ditanam di posisi: " + finalPosition + ". Total coral: " + plantedCoralCount);
//    }

//    private bool IsLeftHandGrabbingCoral()
//    {
//        if (leftHandInteractor != null && leftHandInteractor.hasSelection)
//        {
//            IXRSelectInteractable selected = leftHandInteractor.firstInteractableSelected;
//            if (selected != null)
//            {
//                GameObject selectedObject = selected.transform.gameObject;
//                return selectedObject.CompareTag("Coral");
//            }
//        }

//        return false;
//    }

//    private bool IsTooCloseToOtherCorals(Vector3 position)
//    {
//        foreach (var coral in plantedCorals)
//        {
//            if (Vector3.Distance(position, coral.transform.position) < minDistanceBetweenCorals)
//            {
//                return true; // Terlalu dekat
//            }
//        }

//        return false; // Aman
//    }

//}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;

public class CoralPlacer : MonoBehaviour
{
    [Header("Referensi Coral & Kontrol")]
    public GameObject coralPrefab;
    public InputActionProperty triggerAction;
    public XRBaseInteractor leftHandInteractor;

    [Header("Pengaturan Preview")]
    public float plantingDistance = 2f;
    public float manualOffsetY = 0.5f;
    public Material previewMaterial;
    public Vector3 coralScale = Vector3.one * 1.5f;

    private GameObject coralPreviewInstance;
    private Quaternion previewRotation;
    private Vector3 previewPosition;
    private bool previewActiveLastFrame = false;

    [Header("Batasan Penanaman")]
    public int maxCoralCount = 5;
    public float minDistanceBetweenCorals = 0.5f;

    private int plantedCoralCount = 0;
    private List<GameObject> plantedCorals = new List<GameObject>();

    public CoralPlantingProgressTracker progressTracker;
    public ProgressInfo progressInfo;
    public int sceneLoad;

    [Header("Animation & Sound")]
    public AudioClip plantingSound;
    public Transform coralBounceTarget; // optional: used if bounce target is not the root
    public float bounceScaleAmount = 1.2f;
    public int bounceCount = 5;
    public float initialBounceDuration = 0.3f;


    private void Start()
    {
        coralPreviewInstance = Instantiate(coralPrefab);
        coralPreviewInstance.transform.localScale = coralScale;
        coralPreviewInstance.SetActive(false);

        foreach (var col in coralPreviewInstance.GetComponentsInChildren<Collider>())
            col.enabled = false;

        if (previewMaterial != null)
        {
            foreach (var rend in coralPreviewInstance.GetComponentsInChildren<Renderer>())
                rend.material = previewMaterial;
        }
    }

    private void Update()
    {
        if (!IsLeftHandGrabbingCoral())
        {
            coralPreviewInstance.SetActive(false);
            previewActiveLastFrame = false;
            return;
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, plantingDistance)
            && hit.collider.CompareTag("PlantableArea"))
        {
            coralPreviewInstance.SetActive(true);
            previewPosition = hit.point + new Vector3(0, manualOffsetY, 0);
            coralPreviewInstance.transform.position = previewPosition;

            if (!previewActiveLastFrame)
            {
                previewRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                coralPreviewInstance.transform.rotation = previewRotation;
            }

            previewActiveLastFrame = true;
        }
        else
        {
            coralPreviewInstance.SetActive(false);
            previewActiveLastFrame = false;
        }
    }

    private void OnEnable()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.Enable();
            triggerAction.action.performed += OnTriggerPerformed;
        }
    }

    private void OnDisable()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.performed -= OnTriggerPerformed;
            triggerAction.action.Disable();
        }
    }

    private void OnTriggerPerformed(InputAction.CallbackContext context)
    {
        if (!coralPreviewInstance.activeSelf || !IsLeftHandGrabbingCoral() || plantedCoralCount >= maxCoralCount)
            return;

        if (IsTooCloseToOtherCorals(coralPreviewInstance.transform.position))
            return;

        GameObject newCoral = Instantiate(coralPrefab, coralPreviewInstance.transform.position, coralPreviewInstance.transform.rotation);
        newCoral.transform.localScale = coralScale;

        Transform bounceTarget = coralBounceTarget != null ?
                         newCoral.transform.Find(coralBounceTarget.name) ?? newCoral.transform :
                         newCoral.transform;
        StartCoroutine(PlayBounceAnimation(bounceTarget));
        PlaySoundEffectAt(newCoral.transform.position);

        plantedCorals.Add(newCoral);
        plantedCoralCount++;

        progressInfo.AddCoralProgress(sceneLoad, 1, maxCoralCount);

        if (progressTracker != null)
        {
            progressTracker.IncrementCoral();
        }

        Debug.Log("Coral planted at: " + newCoral.transform.position);
    }

    private bool IsLeftHandGrabbingCoral()
    {
        if (leftHandInteractor != null && leftHandInteractor.hasSelection)
        {
            IXRSelectInteractable selected = leftHandInteractor.firstInteractableSelected;
            return selected != null && selected.transform.CompareTag("Coral");
        }

        return false;
    }

    private bool IsTooCloseToOtherCorals(Vector3 position)
    {
        foreach (var coral in plantedCorals)
        {
            if (Vector3.Distance(position, coral.transform.position) < minDistanceBetweenCorals)
                return true;
        }

        return false;
    }

    private IEnumerator PlayBounceAnimation(Transform target)
    {
        Vector3 originalScale = target.localScale;
        Vector3 targetScale = originalScale * bounceScaleAmount;

        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float easedT = 1 - Mathf.Pow(1 - t, 3); // Cubic ease-out

            target.localScale = Vector3.LerpUnclamped(originalScale, targetScale, easedT);
            yield return null;
        }

        target.localScale = targetScale;
    }


    
    private void PlaySoundEffectAt(Vector3 position)
    {
        if (plantingSound != null)
            AudioSource.PlayClipAtPoint(plantingSound, position);
    }

}
