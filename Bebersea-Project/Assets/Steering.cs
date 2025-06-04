using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SubmarineSteering : MonoBehaviour
{
    [Header("References")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable steeringWheel;
    public Transform wheelTransform; // assign this in Inspector to the actual Wheel object
    public Rigidbody submarineRigidbody;

    [Header("Steering Settings")]
    public float maxSteeringAngle = 45f; // degrees
    public float steeringSpeed = 5f;     // torque scale
    public float testForwardSpeed = 2f;  // forward motion
    public bool invertSteering = false;

    private Quaternion initialWheelLocalRotation;
    private Transform interactorTransform;

    private void Start()
    {
        if (!steeringWheel || !wheelTransform || !submarineRigidbody)
        {
            Debug.LogError("Missing required references.");
            enabled = false;
            return;
        }

        initialWheelLocalRotation = wheelTransform.localRotation;

        // Ensure wheel doesn't get moved by physics
        var rb = wheelTransform.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        steeringWheel.selectEntered.AddListener(OnGrab);
        steeringWheel.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        steeringWheel.selectEntered.RemoveListener(OnGrab);
        steeringWheel.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        interactorTransform = args.interactorObject.transform;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        interactorTransform = null;
    }

    private void Update()
    {
        if (interactorTransform && steeringWheel.isSelected)
        {
            ApplySteeringInput();
        }
    }

    private void ApplySteeringInput()
    {
        // Calculate angle around the wheel's local Z axis
        Vector3 wheelForward = wheelTransform.forward;
        Vector3 wheelUp = wheelTransform.up;

        Vector3 toHand = interactorTransform.position - wheelTransform.position;
        Vector3 handDir = Vector3.ProjectOnPlane(toHand, wheelForward); // Project to rotation plane

        float angle = Vector3.SignedAngle(wheelUp, handDir, wheelForward);
        if (invertSteering) angle *= -1f;

        float clampedAngle = Mathf.Clamp(angle, -maxSteeringAngle, maxSteeringAngle);

        // Apply rotation around local Z
        wheelTransform.localRotation = initialWheelLocalRotation * Quaternion.AngleAxis(clampedAngle, Vector3.forward);

        // Apply submarine yaw torque
        float steeringInput = clampedAngle / maxSteeringAngle;
        float torque = steeringInput * steeringSpeed * (submarineRigidbody.mass / 10f);
        submarineRigidbody.AddTorque(Vector3.up * torque);

        // Optional: move forward slightly when steering
        if (Mathf.Abs(steeringInput) > 0.01f)
        {
            Vector3 forwardForce = transform.forward * testForwardSpeed * submarineRigidbody.mass;
            submarineRigidbody.AddForce(forwardForce);
        }
    }
}
