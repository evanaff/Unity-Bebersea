using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SubController : MonoBehaviour
{
    public XRLever lever;
    public XRKnob knob;

    [Header("Motion Settings")]
    public float maxForwardSpeed = 5f;
    public float maxTurnSpeed = 45f;

    [Header("Inertia Settings")]
    public float forwardAcceleration = 2f;
    public float turnAcceleration = 90f; // degrees/sec²

    private float currentForwardSpeed = 0f;
    private float currentTurnSpeed = 0f;

    public ProgressInfo progressInfo;

    void Start()
    {
        progressInfo.LoadMapSubmarineTransform(transform);
    }

    void Update()
    {
        // Target speeds based on input
        float targetForwardSpeed = lever.value ? maxForwardSpeed : 0f;
        float turnInput = Mathf.Lerp(-1f, 1f, knob.value); // [-1, 1]
        float targetTurnSpeed = turnInput * maxTurnSpeed;

        // Smooth acceleration/deceleration
        currentForwardSpeed = Mathf.MoveTowards(currentForwardSpeed, targetForwardSpeed, forwardAcceleration * Time.deltaTime);
        currentTurnSpeed = Mathf.MoveTowards(currentTurnSpeed, targetTurnSpeed, turnAcceleration * Time.deltaTime);

        // Apply movement and rotation
        transform.position += transform.forward * currentForwardSpeed * Time.deltaTime;
        transform.Rotate(0f, currentTurnSpeed * Time.deltaTime, 0f);
    }
}
