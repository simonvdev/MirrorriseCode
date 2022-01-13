using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private Rigidbody _rb = null;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField]
    private float xMaxRotation = 90;
    [SerializeField]
    private float xMinRotation = -90;
    private Vector2 _mouseLook;


    private void Start()
    {
        _rb = transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        _mouseLook += mouseInput * mouseSensitivity;

        _mouseLook.y = Mathf.Clamp(_mouseLook.y, xMinRotation, xMaxRotation);

        transform.localRotation = Quaternion.Euler(-_mouseLook.y,0f,0f);

        _rb.rotation = Quaternion.AngleAxis(_mouseLook.x, Vector3.up);
    }
}
