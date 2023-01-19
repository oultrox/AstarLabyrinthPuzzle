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
    private Vector3 _moveDirection;
    private Vector3 _movement;
    private bool _isTiltActivated = false;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Movement"];
        ActivateTiltMovement();
    }

    private void Update()
    {
        MoveCharacter();

        //Just for fun.
        if(_isTiltActivated && _moveAction.ReadValue<Vector2>() == Vector2.zero)
        {
            TiltMovement();
        }
        
    }

    private void ActivateTiltMovement()
    {
        if (SystemInfo.supportsGyroscope)
        {
            _isTiltActivated = !_isTiltActivated;
            Input.gyro.enabled = _isTiltActivated;
        }
    }

    private void TiltMovement()
    {
        _moveDirection = new Vector3(Input.acceleration.x, 0, Input.acceleration.y);
        _moveDirection.y = _gravity;
        _characterController.Move(_speed * Time.deltaTime * _moveDirection);
    }


    private void MoveCharacter()
    {
        _moveDirection = _moveAction.ReadValue<Vector2>();


        _movement = new Vector3(_moveDirection.x, 0, _moveDirection.y);
        _movement.y = _gravity;
        _characterController.Move(_speed * Time.deltaTime * _movement);
    }
}
