using UnityEngine;

public class PanoramicImageCameraController : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    [Header("Spherical viewing")]
    public float rotateSpeed;
    public float zoomSpeed = 2500.0f;
    public float zoomAmount = 0.0f;

    [Header("Camera panning")]
    public float panSpeed = 10f;
    public float screenEdgeBuffer = 0.05f; // 5% of the screen

    private void Awake() => ConfigureSettings();

    void Update()
    {
        HandleInteractiveCamera();
        HandleCameraPan();
    }

    public void DisplayImage(Texture sprite)
    {
        meshRenderer.material.mainTexture = sprite;
        ResetZoom();
    } 

    private void ResetZoom() => zoomAmount = 0;

    private void HandleInteractiveCamera()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles = new Vector3(
                transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * rotateSpeed,
                transform.localEulerAngles.y - Input.GetAxis("Mouse X") * rotateSpeed, 0);

            if (transform.localEulerAngles.x < 315 && transform.localEulerAngles.x > 180)
                transform.localEulerAngles = new Vector3(315, transform.localEulerAngles.y, transform.localEulerAngles.z);
            else if (transform.localEulerAngles.x > 45 && transform.localEulerAngles.x < 180)
                transform.localEulerAngles = new Vector3(45, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        zoomAmount = Mathf.Clamp(zoomAmount + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed, -5.0f, 5.0f);
        Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
    }

    private void HandleCameraPan()
    {
        Vector3 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;

        if (mousePos.x >= screenWidth * (1 - screenEdgeBuffer)) // Right edge
            transform.localEulerAngles += new Vector3(0, panSpeed * Time.deltaTime, 0);
        else if (mousePos.x <= screenWidth * screenEdgeBuffer) // Left edge
            transform.localEulerAngles -= new Vector3(0, panSpeed * Time.deltaTime, 0);
    }

    private void ConfigureSettings()
    {
        rotateSpeed = Settings.GetImageViewSensitivity;
    }
}