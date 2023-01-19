using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float _speed = 6.0f;
    [SerializeField] float _gravity = -20f;
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Vector2 _moveDirection;
    private Vector3 _movement;
    private bool _isTiltActivated = true;
    private Quaternion _tiltRotation;
    private Quaternion currentRotation;
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Movement"];
        ActivateTiltMovement();
    }

    private void Update()
    {
        if(!_isTiltActivated)
        {
            MoveCharacter();
        }
        else
        {
            TiltMovement();
        }

    }

    private void TiltMovement()
    {
        currentRotation = Input.gyro.attitude;

        // Set the rotation of the character controller to match the device's current rotation
        _characterController.transform.rotation = currentRotation;

        // Calculate the movement direction based on the tilt of the device
        _moveDirection = new Vector3(Input.acceleration.x, 0, Input.acceleration.y);

        // Apply gravity to the movement direction
        _moveDirection.y -= _gravity * Time.deltaTime;

        // Move the character controller based on the calculated movement direction
        _characterController.Move(_moveDirection * _speed * Time.deltaTime);
    }

    private void ActivateTiltMovement()
    {
        if (SystemInfo.supportsGyroscope)
        {
            _isTiltActivated = !_isTiltActivated;
            Input.gyro.enabled = _isTiltActivated;
            _tiltRotation = Input.gyro.attitude;
        }
    }

    

    private void MoveCharacter()
    {
        _moveDirection = _moveAction.ReadValue<Vector2>();


        _movement = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        _movement.y = _gravity;
        _characterController.Move(_speed * Time.deltaTime * _movement);
    }
}
