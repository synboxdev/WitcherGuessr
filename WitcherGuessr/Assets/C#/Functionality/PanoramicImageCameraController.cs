using UnityEngine;

public class PanoramicImageCameraController : MonoBehaviour
{
    public float rotateSpeed = 4.0f;
    public float zoomSpeed = 2500.0f;
    public float zoomAmount = 0.0f;

    public MeshRenderer meshRenderer;

    void Update()
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

    public void DisplayImage(Texture sprite) => meshRenderer.material.mainTexture = sprite;
}