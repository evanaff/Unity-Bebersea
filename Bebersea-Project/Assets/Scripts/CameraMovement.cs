using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 15f;
    public Transform playerBody; // Referensi ke objek pemain (GameObject utama)

    float rotationX = 0f;

    void Start()
    {
        // Kunci kursor ke tengah layar dan sembunyikan
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Reset rotasi kamera secara eksplisit
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        rotationX = 0f;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Rotasi kamera vertikal (atas-bawah)
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Rotasi badan pemain horizontal (kiri-kanan)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
