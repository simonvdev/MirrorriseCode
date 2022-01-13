using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.zero;
    [SerializeField] private float rotateSpeed = 30f;
    [SerializeField] private bool canRotate = true;

    public void StartRotating()
    {
        canRotate = true;
    }

    public void StopRotating()
    {
        canRotate = false;
    }

    private void Update()
    {
        if (canRotate)
        { 
            transform.Rotate(axis, rotateSpeed * Time.deltaTime, Space.World);
        }     
    }

}
