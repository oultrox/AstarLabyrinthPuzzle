using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float _speed = 6.0f;
    [SerializeField] float _gravity = -20f;
    private CharacterController _charController;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private Vector2 moveDirection;
    private Vector3 movement;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Movement"];
    }

    void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {

        moveDirection = _moveAction.ReadValue<Vector2>();
        //float deltaX = Input.GetAxis("Horizontal") * _speed;
        //float deltaZ = Input.GetAxis("Vertical") * _speed;

        movement = new Vector3(moveDirection.x, 0, moveDirection.y);
        movement.y = _gravity;

        _charController.Move(_speed * Time.deltaTime * movement);
    }
}
