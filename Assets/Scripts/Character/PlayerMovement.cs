using System;
using UnityEngine;

namespace Character
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float jumpForce = 50f;
        [SerializeField] private float mGroundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayerMask = 0;

        private bool _grounded;
        
        private Rigidbody _rigidbody;
        private Transform _camera;

        private Vector3 _movement;
        private Vector3 _inputVector;
        private bool _jumped;


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = transform.GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            _inputVector = new Vector3(Input.GetAxis("MovementHorizontal"), 0, Input.GetAxis("MovementVertical"));
            _inputVector.Normalize();
            
            if(!_jumped)
                _jumped = Input.GetButtonDown("Jump");
        }

        private void FixedUpdate()
        {
            HandleMovement();
            Jump();
        }

        private void Jump()
        {
            _grounded = Physics.CheckSphere(transform.position, mGroundCheckRadius, groundLayerMask);

            if (_jumped && _grounded)
            {
                _jumped = false;
                _rigidbody.AddForce(Vector3.up * jumpForce);
            }
        }

        private void HandleMovement()
        {
            _movement = transform.TransformDirection(_inputVector) * (moveSpeed * Time.fixedDeltaTime);
            
            if (_rigidbody) _rigidbody.velocity = new Vector3(_movement.x, _rigidbody.velocity.y, _movement.z);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, mGroundCheckRadius);
        }
    }
}
